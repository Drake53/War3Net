// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Extension methods for <see cref="BinaryWriter"/>.
    /// </summary>
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Writes a null-terminated string.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The string to write.</param>
        public static void WriteCString(this BinaryWriter writer, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(value);
            writer.Write(bytes);
            writer.Write((byte)0);
        }

        /// <summary>
        /// Writes a null-terminated string with a fixed length.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The string to write.</param>
        /// <param name="length">The fixed length.</param>
        public static void WriteCString(this BinaryWriter writer, string value, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bytes = new byte[length];
            var stringBytes = System.Text.Encoding.UTF8.GetBytes(value);
            Array.Copy(stringBytes, bytes, Math.Min(stringBytes.Length, length - 1));
            writer.Write(bytes);
        }

        /// <summary>
        /// Writes a big-endian UInt16.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteUInt16BE(this BinaryWriter writer, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            writer.Write(bytes);
        }

        /// <summary>
        /// Writes a big-endian UInt32.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteUInt32BE(this BinaryWriter writer, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            writer.Write(bytes);
        }

        /// <summary>
        /// Writes a big-endian UInt64.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteUInt64BE(this BinaryWriter writer, ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            writer.Write(bytes);
        }

        /// <summary>
        /// Writes a variable-length big-endian integer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="byteCount">The number of bytes to write.</param>
        public static void WriteBigEndianValue(this BinaryWriter writer, ulong value, int byteCount)
        {
            if (byteCount > 8)
            {
                throw new ArgumentException("Cannot write more than 8 bytes from a ulong.", nameof(byteCount));
            }

            var bytes = new byte[byteCount];
            for (int i = byteCount - 1; i >= 0; i--)
            {
                bytes[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            writer.Write(bytes);
        }

        /// <summary>
        /// Writes a content key.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="cKey">The content key.</param>
        public static void WriteCKey(this BinaryWriter writer, CascKey cKey)
        {
            writer.Write(cKey.ToArray());
        }

        /// <summary>
        /// Writes an encoded key.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="eKey">The encoded key.</param>
        public static void WriteEKey(this BinaryWriter writer, EKey eKey)
        {
            writer.Write(eKey.ToArray());
        }

        /// <summary>
        /// Writes an encoded key with a specific length.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="eKey">The encoded key.</param>
        /// <param name="length">The length to write.</param>
        public static void WriteEKey(this BinaryWriter writer, EKey eKey, int length)
        {
            var bytes = eKey.ToArray();
            if (bytes.Length < length)
            {
                Array.Resize(ref bytes, length);
            }

            writer.Write(bytes, 0, length);
        }

        /// <summary>
        /// Writes an MD5 hash.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="hash">The MD5 hash.</param>
        public static void WriteMD5Hash(this BinaryWriter writer, byte[] hash)
        {
            if (hash == null || hash.Length != CascConstants.MD5HashSize)
            {
                throw new ArgumentException($"MD5 hash must be exactly {CascConstants.MD5HashSize} bytes.", nameof(hash));
            }

            writer.Write(hash);
        }

        /// <summary>
        /// Writes a SHA1 hash.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="hash">The SHA1 hash.</param>
        public static void WriteSHA1Hash(this BinaryWriter writer, byte[] hash)
        {
            if (hash == null || hash.Length != CascConstants.SHA1HashSize)
            {
                throw new ArgumentException($"SHA1 hash must be exactly {CascConstants.SHA1HashSize} bytes.", nameof(hash));
            }

            writer.Write(hash);
        }

        /// <summary>
        /// Writes padding bytes.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="count">The number of padding bytes to write.</param>
        public static void WritePadding(this BinaryWriter writer, int count)
        {
            writer.Write(new byte[count]);
        }
    }
}