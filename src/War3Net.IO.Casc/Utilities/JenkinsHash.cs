// ------------------------------------------------------------------------------
// <copyright file="JenkinsHash.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Jenkins hash implementation for CASC validation.
    /// </summary>
    public static class JenkinsHash
    {
        /// <summary>
        /// Computes the Jenkins "hashlittle2" hash.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <param name="pc">The first hash output.</param>
        /// <param name="pb">The second hash output.</param>
        public static void HashLittle2(byte[] data, out uint pc, out uint pb)
        {
            HashLittle2(data, 0, data?.Length ?? 0, out pc, out pb);
        }

        /// <summary>
        /// Computes the Jenkins "hashlittle2" hash.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <param name="offset">The offset in the data array.</param>
        /// <param name="length">The length of data to hash.</param>
        /// <param name="pc">The first hash output.</param>
        /// <param name="pb">The second hash output.</param>
        public static void HashLittle2(byte[] data, int offset, int length, out uint pc, out uint pb)
        {
            uint a, b, c;

            // Set up the internal state
            a = b = c = 0xdeadbeef + (uint)length;
            pc = pb = 0;

            if (data == null || length == 0)
            {
                pc = c;
                pb = b;
                return;
            }

            // Validate bounds
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
            }

            if (offset + length > data.Length)
            {
                throw new ArgumentException($"Offset ({offset}) + Length ({length}) exceeds array bounds ({data.Length}).");
            }

            int pos = offset;

            // Handle most of the data
            while (length > 12)
            {
                a += BitConverter.ToUInt32(data, pos);
                b += BitConverter.ToUInt32(data, pos + 4);
                c += BitConverter.ToUInt32(data, pos + 8);
                Mix(ref a, ref b, ref c);
                pos += 12;
                length -= 12;
            }

            // Handle the last (less than 12) bytes
            c += (uint)length;
            switch (length)
            {
                case 12:
                    c += (uint)data[pos + 11] << 24;
                    goto case 11;
                case 11:
                    c += (uint)data[pos + 10] << 16;
                    goto case 10;
                case 10:
                    c += (uint)data[pos + 9] << 8;
                    goto case 9;
                case 9:
                    c += data[pos + 8];
                    goto case 8;
                case 8:
                    b += (uint)data[pos + 7] << 24;
                    goto case 7;
                case 7:
                    b += (uint)data[pos + 6] << 16;
                    goto case 6;
                case 6:
                    b += (uint)data[pos + 5] << 8;
                    goto case 5;
                case 5:
                    b += data[pos + 4];
                    goto case 4;
                case 4:
                    a += (uint)data[pos + 3] << 24;
                    goto case 3;
                case 3:
                    a += (uint)data[pos + 2] << 16;
                    goto case 2;
                case 2:
                    a += (uint)data[pos + 1] << 8;
                    goto case 1;
                case 1:
                    a += data[pos];
                    break;
                case 0:
                    pc = c;
                    pb = b;
                    return;
            }

            Final(ref a, ref b, ref c);
            pc = c;
            pb = b;
        }

        /// <summary>
        /// Computes the Jenkins "hashlittle" hash.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <returns>The hash value.</returns>
        public static uint HashLittle(byte[] data)
        {
            return HashLittle(data, 0, data?.Length ?? 0);
        }

        /// <summary>
        /// Computes the Jenkins "hashlittle" hash.
        /// </summary>
        /// <param name="data">The data to hash.</param>
        /// <param name="offset">The offset in the data array.</param>
        /// <param name="length">The length of data to hash.</param>
        /// <returns>The hash value.</returns>
        public static uint HashLittle(byte[] data, int offset, int length)
        {
            HashLittle2(data, offset, length, out uint pc, out _);
            return pc;
        }

        private static void Mix(ref uint a, ref uint b, ref uint c)
        {
            a -= c; a ^= Rot(c, 4); c += b;
            b -= a; b ^= Rot(a, 6); a += c;
            c -= b; c ^= Rot(b, 8); b += a;
            a -= c; a ^= Rot(c, 16); c += b;
            b -= a; b ^= Rot(a, 19); a += c;
            c -= b; c ^= Rot(b, 4); b += a;
        }

        private static void Final(ref uint a, ref uint b, ref uint c)
        {
            c ^= b; c -= Rot(b, 14);
            a ^= c; a -= Rot(c, 11);
            b ^= a; b -= Rot(a, 25);
            c ^= b; c -= Rot(b, 16);
            a ^= c; a -= Rot(c, 4);
            b ^= a; b -= Rot(a, 14);
            c ^= b; c -= Rot(b, 24);
        }

        private static uint Rot(uint x, int k)
        {
            return (x << k) | (x >> (32 - k));
        }
    }
}