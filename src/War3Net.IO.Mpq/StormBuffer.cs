// ------------------------------------------------------------------------------
// <copyright file="StormBuffer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Mpq
{
    internal class StormBuffer
    {
        private static readonly Lazy<StormBuffer> _stormBuffer = new Lazy<StormBuffer>(() => new StormBuffer());

        private readonly uint[] _buffer;

        private StormBuffer()
        {
            uint seed = 0x100001;

            _buffer = new uint[0x500];

            for (uint index1 = 0; index1 < 0x100; index1++)
            {
                var index2 = index1;
                for (var i = 0; i < 5; i++, index2 += 0x100)
                {
                    seed = ((seed * 125) + 3) % 0x2aaaab;
                    var temp = (seed & 0xffff) << 16;
                    seed = ((seed * 125) + 3) % 0x2aaaab;

                    _buffer[index2] = temp | (seed & 0xffff);
                }
            }
        }

        private static uint[] Buffer => _stormBuffer.Value._buffer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input">The input string for which a hash value should be generated.</param>
        /// <param name="offset">A key to generate different values for the same string. Values commonly used are 0, 0x100, 0x200, and 0x300.</param>
        /// <returns>A hashed value for the given <paramref name="input"/>.</returns>
        internal static uint HashString(string input, int offset)
        {
            uint seed1 = 0x7fed7fed;
            var seed2 = 0xeeeeeeee;

            foreach (var c in input.ToUpper())
            {
                var val = (int)c;
                seed1 = Buffer[offset + val] ^ (seed1 + seed2);
                seed2 = (uint)val + seed1 + seed2 + (seed2 << 5) + 3;
            }

            return seed1;
        }

        internal static bool TryGetHashString(string input, int offset, out uint hash)
        {
            hash = 0x7fed7fed; // seed1

            // StormBuffer size is 0x500, offset can be 0, 0x100, 0x200, or 0x300, so characters' numerical value cannot be 0x200 or higher.
            if (input.Any(c => c >= 0x200))
            {
                return false;
            }

            var seed2 = 0xeeeeeeee;
            foreach (var c in input.ToUpper())
            {
                var val = (int)c;
                hash = Buffer[offset + val] ^ (hash + seed2);
                seed2 = (uint)val + hash + seed2 + (seed2 << 5) + 3;
            }

            return true;
        }

        internal static byte[] EncryptStream(Stream stream, uint seed1, int offset, int length)
        {
            var data = new byte[length];
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.Read(data, 0, length) != length)
            {
                throw new Exception("Insufficient data or invalid data length");
            }

            EncryptBlock(data, seed1);
            return data;
        }

        internal static void EncryptBlock(byte[] data, uint seed1)
        {
            var seed2 = 0xeeeeeeee;

            // NB: If the block is not an even multiple of 4,
            // the remainder is not encrypted
            for (var i = 0; i < data.Length - 3; i += 4)
            {
                seed2 += Buffer[0x400 + (seed1 & 0xff)];

                var unencrypted = BitConverter.ToUInt32(data, i);
                var result = unencrypted ^ (seed1 + seed2);

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = unencrypted + seed2 + (seed2 << 5) + 3;

                data[i + 0] = (byte)(result & 0xff);
                data[i + 1] = (byte)((result >> 8) & 0xff);
                data[i + 2] = (byte)((result >> 16) & 0xff);
                data[i + 3] = (byte)((result >> 24) & 0xff);
            }
        }

        internal static void EncryptBlock(uint[] data, uint seed1)
        {
            var seed2 = 0xeeeeeeee;

            for (var i = 0; i < data.Length; i++)
            {
                seed2 += Buffer[0x400 + (seed1 & 0xff)];

                var unencrypted = data[i];
                var result = unencrypted ^ (seed1 + seed2);

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = unencrypted + seed2 + (seed2 << 5) + 3;

                data[i] = result;
            }
        }

        internal static void DecryptBlock(byte[] data, uint seed1)
        {
            var seed2 = 0xeeeeeeee;

            // NB: If the block is not an even multiple of 4,
            // the remainder is not encrypted
            for (var i = 0; i < data.Length - 3; i += 4)
            {
                seed2 += Buffer[0x400 + (seed1 & 0xff)];

                var result = BitConverter.ToUInt32(data, i);
                result ^= seed1 + seed2;

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;

                data[i + 0] = (byte)(result & 0xff);
                data[i + 1] = (byte)((result >> 8) & 0xff);
                data[i + 2] = (byte)((result >> 16) & 0xff);
                data[i + 3] = (byte)((result >> 24) & 0xff);
            }
        }

        internal static void DecryptBlock(uint[] data, uint seed1)
        {
            var seed2 = 0xeeeeeeee;

            for (var i = 0; i < data.Length; i++)
            {
                seed2 += Buffer[0x400 + (seed1 & 0xff)];
                var result = data[i];
                result ^= seed1 + seed2;

                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;
                data[i] = result;
            }
        }

        // This function calculates the encryption key based on
        // some assumptions we can make about the headers for encrypted files
        internal static bool DetectFileSeed(uint value0, uint value1, uint decrypted, out uint detectedSeed)
        {
            var temp = (value0 ^ decrypted) - 0xeeeeeeee;

            for (var i = 0; i < 0x100; i++)
            {
                var seed1 = temp - Buffer[0x400 + i];
                var seed2 = 0xeeeeeeee + Buffer[0x400 + (seed1 & 0xff)];
                var result = value0 ^ (seed1 + seed2);

                if (result != decrypted)
                {
                    continue;
                }

                detectedSeed = seed1;

                // Test this result against the 2nd value
                seed1 = ((~seed1 << 21) + 0x11111111) | (seed1 >> 11);
                seed2 = result + seed2 + (seed2 << 5) + 3;

                seed2 += Buffer[0x400 + (seed1 & 0xff)];
                result = value1 ^ (seed1 + seed2);

                if ((result & 0xfffc0000) == 0)
                {
                    return true;
                }
            }

            detectedSeed = 0;
            return false;
        }

        internal static IEnumerable<uint> DetectFileSeeds(uint value0, uint decrypted)
        {
            var temp = (value0 ^ decrypted) - 0xeeeeeeee;

            for (var i = 0; i < 0x100; i++)
            {
                var seed1 = temp - Buffer[0x400 + i];
                var seed2 = 0xeeeeeeee + Buffer[0x400 + (seed1 & 0xff)];
                var result = value0 ^ (seed1 + seed2);

                if (result == decrypted)
                {
                    yield return seed1;
                }
            }
        }
    }
}