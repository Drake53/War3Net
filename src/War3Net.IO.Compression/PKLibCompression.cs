// ------------------------------------------------------------------------------
// <copyright file="PKLibCompression.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1810 // Initialize reference type static fields inline

using System;
using System.IO;

namespace War3Net.IO.Compression
{
    /// <summary>
    /// Provides methods to decompress PKLib compressed data.
    /// </summary>
    public static class PKLibCompression
    {
        private static readonly byte[] _sPosition1;
        private static readonly byte[] _sPosition2;

        private static readonly byte[] _sLenBits =
        {
            3, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7,
        };

        private static readonly byte[] _sLenCode =
        {
            5, 3, 1, 6, 10, 2, 12, 20, 4, 24, 8, 48, 16, 32, 64, 0,
        };

        private static readonly byte[] _sExLenBits =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8,
        };

        private static readonly ushort[] _sLenBase =
        {
            0x0000, 0x0001, 0x0002, 0x0003, 0x0004, 0x0005, 0x0006, 0x0007,
            0x0008, 0x000A, 0x000E, 0x0016, 0x0026, 0x0046, 0x0086, 0x0106,
        };

        private static readonly byte[] _sDistBits =
        {
            2, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
        };

        private static readonly byte[] _sDistCode =
        {
            0x03, 0x0D, 0x05, 0x19, 0x09, 0x11, 0x01, 0x3E, 0x1E, 0x2E, 0x0E, 0x36, 0x16, 0x26, 0x06, 0x3A,
            0x1A, 0x2A, 0x0A, 0x32, 0x12, 0x22, 0x42, 0x02, 0x7C, 0x3C, 0x5C, 0x1C, 0x6C, 0x2C, 0x4C, 0x0C,
            0x74, 0x34, 0x54, 0x14, 0x64, 0x24, 0x44, 0x04, 0x78, 0x38, 0x58, 0x18, 0x68, 0x28, 0x48, 0x08,
            0xF0, 0x70, 0xB0, 0x30, 0xD0, 0x50, 0x90, 0x10, 0xE0, 0x60, 0xA0, 0x20, 0xC0, 0x40, 0x80, 0x00,
        };

        static PKLibCompression()
        {
            _sPosition1 = GenerateDecodeTable(_sDistBits, _sDistCode);
            _sPosition2 = GenerateDecodeTable(_sLenBits, _sLenCode);
        }

        /// <summary>
        /// Decompresses the input data.
        /// </summary>
        /// <param name="data">Byte array containing compressed data.</param>
        /// <param name="expectedLength">The expected length (in bytes) of the decompressed data.</param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(byte[] data, uint expectedLength)
        {
            using var memoryStream = new MemoryStream(data);
            return Decompress(memoryStream, expectedLength);
        }

        /// <summary>
        /// Decompresses the input stream.
        /// </summary>
        /// <param name="data">Stream containing compressed data.</param>
        /// <param name="expectedLength">The expected length (in bytes) of the decompressed data.</param>
        /// <returns>Byte array containing the decompressed data.</returns>
        public static byte[] Decompress(Stream data, uint expectedLength)
        {
            using var bitstream = new BitStream(data ?? throw new ArgumentNullException(nameof(data)));

            var compressionType = (PKLibCompressionType)data.ReadByte();
            if (compressionType != PKLibCompressionType.Binary && compressionType != PKLibCompressionType.Ascii)
            {
                throw new InvalidDataException($"Invalid compression type: {compressionType}");
            }

            var dictSizeBits = data.ReadByte();

            // This is 6 in test cases
            if (dictSizeBits < 4 || dictSizeBits > 6)
            {
                throw new InvalidDataException($"Invalid dictionary size: {dictSizeBits}");
            }

            var outputbuffer = new byte[expectedLength];
            Stream outputstream = new MemoryStream(outputbuffer);

            int instruction;
            while ((instruction = DecodeLit(bitstream, compressionType)) != -1)
            {
                if (instruction < 0x100)
                {
                    outputstream.WriteByte((byte)instruction);
                }
                else
                {
                    // If instruction is greater than 0x100, it means "Repeat n - 0xFE bytes"
                    var copylength = instruction - 0xFE;
                    var moveback = DecodeDist(bitstream, dictSizeBits, copylength);
                    if (moveback == 0)
                    {
                        break;
                    }

                    var source = (int)outputstream.Position - moveback;

                    // We can't just outputstream.Write the section of the array
                    // because it might overlap with what is currently being written
                    while (copylength-- > 0)
                    {
                        outputstream.WriteByte(outputbuffer[source++]);
                    }
                }
            }

            if (outputstream.Position == expectedLength)
            {
                return outputbuffer;
            }
            else
            {
                // Resize the array
                var result = new byte[outputstream.Position];
                Array.Copy(outputbuffer, 0, result, 0, result.Length);
                return result;
            }
        }

        private static byte[] GenerateDecodeTable(byte[] bits, byte[] codes)
        {
            var result = new byte[256];

            for (var i = bits.Length - 1; i >= 0; i--)
            {
                uint idx1 = codes[i];
                var idx2 = 1U << bits[i];

                do
                {
                    result[idx1] = (byte)i;
                    idx1 += idx2;
                }
                while (idx1 < 0x100);
            }

            return result;
        }

        // Return values:
        // 0x000 - 0x0FF : One byte from compressed file.
        // 0x100 - 0x305 : Copy previous block (0x100 = 1 byte)
        // -1            : EOF
        private static int DecodeLit(BitStream input, PKLibCompressionType compressionType)
        {
            switch (input.ReadBits(1))
            {
                case -1:
                    return -1;

                case 1:
                    // The next bits are position in buffers
                    int pos = _sPosition2[input.PeekByte()];

                    // Skip the bits we just used
                    if (input.ReadBits(_sLenBits[pos]) == -1)
                    {
                        return -1;
                    }

                    int nbits = _sExLenBits[pos];
                    if (nbits != 0)
                    {
                        // TODO: Verify this conversion
                        var val2 = input.ReadBits(nbits);
                        if (val2 == -1 && (pos + val2 != 0x10e))
                        {
                            return -1;
                        }

                        pos = _sLenBase[pos] + val2;
                    }

                    return pos + 0x100; // Return number of bytes to repeat

                case 0:
                    if (compressionType == PKLibCompressionType.Binary)
                    {
                        return input.ReadBits(8);
                    }

                    // TODO: Text mode
                    throw new NotImplementedException($"Text mode (compression of type {PKLibCompressionType.Ascii}) is not yet implemented");

                default:
                    return 0;
            }
        }

        private static int DecodeDist(BitStream input, int dictSizeBits, int length)
        {
            if (input.EnsureBits(8) == false)
            {
                return 0;
            }

            int pos = _sPosition1[input.PeekByte()];
            var skip = _sDistBits[pos];     // Number of bits to skip

            // Skip the appropriate number of bits
            if (input.ReadBits(skip) == -1)
            {
                return 0;
            }

            if (length == 2)
            {
                if (input.EnsureBits(2) == false)
                {
                    return 0;
                }

                pos = (pos << 2) | input.ReadBits(2);
            }
            else
            {
                if (input.EnsureBits(dictSizeBits) == false)
                {
                    return 0;
                }

                pos = (pos << dictSizeBits) | input.ReadBits(dictSizeBits);
            }

            return pos + 1;
        }
    }
}