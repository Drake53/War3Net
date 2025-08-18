// ------------------------------------------------------------------------------
// <copyright file="EndianConverter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Provides methods for converting between big-endian and little-endian byte order.
    /// </summary>
    public static class EndianConverter
    {
        /// <summary>
        /// Swaps the byte order of a 16-bit integer.
        /// </summary>
        /// <param name="value">The value to swap.</param>
        /// <returns>The swapped value.</returns>
        public static ushort Swap(ushort value)
        {
            return (ushort)((value >> 8) | (value << 8));
        }

        /// <summary>
        /// Swaps the byte order of a 32-bit integer.
        /// </summary>
        /// <param name="value">The value to swap.</param>
        /// <returns>The swapped value.</returns>
        public static uint Swap(uint value)
        {
            return ((value & 0x000000FF) << 24) |
                   ((value & 0x0000FF00) << 8) |
                   ((value & 0x00FF0000) >> 8) |
                   ((value & 0xFF000000) >> 24);
        }

        /// <summary>
        /// Swaps the byte order of a 64-bit integer.
        /// </summary>
        /// <param name="value">The value to swap.</param>
        /// <returns>The swapped value.</returns>
        public static ulong Swap(ulong value)
        {
            return ((value & 0x00000000000000FF) << 56) |
                   ((value & 0x000000000000FF00) << 40) |
                   ((value & 0x0000000000FF0000) << 24) |
                   ((value & 0x00000000FF000000) << 8) |
                   ((value & 0x000000FF00000000) >> 8) |
                   ((value & 0x0000FF0000000000) >> 24) |
                   ((value & 0x00FF000000000000) >> 40) |
                   ((value & 0xFF00000000000000) >> 56);
        }

        /// <summary>
        /// Converts a big-endian byte array to a little-endian value.
        /// </summary>
        /// <param name="bytes">The big-endian bytes.</param>
        /// <returns>The little-endian value.</returns>
        public static ushort FromBigEndian16(byte[] bytes)
        {
            if (bytes.Length < 2)
            {
                throw new ArgumentException("Array must contain at least 2 bytes.", nameof(bytes));
            }

            return (ushort)((bytes[0] << 8) | bytes[1]);
        }

        /// <summary>
        /// Converts a big-endian byte array to a little-endian value.
        /// </summary>
        /// <param name="bytes">The big-endian bytes.</param>
        /// <returns>The little-endian value.</returns>
        public static uint FromBigEndian32(byte[] bytes)
        {
            if (bytes.Length < 4)
            {
                throw new ArgumentException("Array must contain at least 4 bytes.", nameof(bytes));
            }

            return ((uint)bytes[0] << 24) |
                   ((uint)bytes[1] << 16) |
                   ((uint)bytes[2] << 8) |
                   bytes[3];
        }

        /// <summary>
        /// Converts a big-endian byte array to a little-endian value.
        /// </summary>
        /// <param name="bytes">The big-endian bytes.</param>
        /// <returns>The little-endian value.</returns>
        public static ulong FromBigEndian64(byte[] bytes)
        {
            if (bytes.Length < 8)
            {
                throw new ArgumentException("Array must contain at least 8 bytes.", nameof(bytes));
            }

            return ((ulong)bytes[0] << 56) |
                   ((ulong)bytes[1] << 48) |
                   ((ulong)bytes[2] << 40) |
                   ((ulong)bytes[3] << 32) |
                   ((ulong)bytes[4] << 24) |
                   ((ulong)bytes[5] << 16) |
                   ((ulong)bytes[6] << 8) |
                   bytes[7];
        }

        /// <summary>
        /// Converts a little-endian value to big-endian bytes.
        /// </summary>
        /// <param name="value">The little-endian value.</param>
        /// <returns>The big-endian bytes.</returns>
        public static byte[] ToBigEndian(ushort value)
        {
            return new byte[]
            {
                (byte)(value >> 8),
                (byte)value,
            };
        }

        /// <summary>
        /// Converts a little-endian value to big-endian bytes.
        /// </summary>
        /// <param name="value">The little-endian value.</param>
        /// <returns>The big-endian bytes.</returns>
        public static byte[] ToBigEndian(uint value)
        {
            return new byte[]
            {
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value,
            };
        }

        /// <summary>
        /// Converts a little-endian value to big-endian bytes.
        /// </summary>
        /// <param name="value">The little-endian value.</param>
        /// <returns>The big-endian bytes.</returns>
        public static byte[] ToBigEndian(ulong value)
        {
            return new byte[]
            {
                (byte)(value >> 56),
                (byte)(value >> 48),
                (byte)(value >> 40),
                (byte)(value >> 32),
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value,
            };
        }

        /// <summary>
        /// Reads a variable-length big-endian value from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="offset">The offset to start reading from.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The value.</returns>
        public static ulong ReadBigEndianValue(byte[] bytes, int offset, int length)
        {
            if (length > 8)
            {
                throw new ArgumentException("Cannot read more than 8 bytes into a ulong.", nameof(length));
            }

            ulong value = 0;
            for (int i = 0; i < length; i++)
            {
                value = (value << 8) | bytes[offset + i];
            }

            return value;
        }
    }
}