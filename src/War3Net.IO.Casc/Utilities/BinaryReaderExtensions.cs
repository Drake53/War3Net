// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

using War3Net.IO.Casc.Structures;

namespace War3Net.IO.Casc.Utilities
{
    /// <summary>
    /// Extension methods for <see cref="BinaryReader"/>.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a null-terminated string.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The string.</returns>
        public static string ReadCString(this BinaryReader reader)
        {
            var sb = new StringBuilder();
            char ch;
            while ((ch = reader.ReadChar()) != '\0')
            {
                sb.Append(ch);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads a null-terminated string with a maximum length.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The string.</returns>
        public static string ReadCString(this BinaryReader reader, int maxLength)
        {
            var bytes = reader.ReadBytes(maxLength);
            var nullIndex = Array.IndexOf(bytes, (byte)0);
            if (nullIndex >= 0)
            {
                return Encoding.UTF8.GetString(bytes, 0, nullIndex);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Reads a big-endian UInt16.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The value.</returns>
        public static ushort ReadUInt16BE(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(2);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a big-endian UInt32.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The value.</returns>
        public static uint ReadUInt32BE(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a big-endian UInt64.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The value.</returns>
        public static ulong ReadUInt64BE(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(8);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reads a variable-length big-endian integer.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <param name="byteCount">The number of bytes to read.</param>
        /// <returns>The value.</returns>
        public static ulong ReadBigEndianValue(this BinaryReader reader, int byteCount)
        {
            if (byteCount > 8)
            {
                throw new ArgumentException("Cannot read more than 8 bytes into a ulong.", nameof(byteCount));
            }

            ulong value = 0;
            for (int i = 0; i < byteCount; i++)
            {
                value = (value << 8) | reader.ReadByte();
            }

            return value;
        }

        /// <summary>
        /// Reads a content key.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The content key.</returns>
        public static CascKey ReadCKey(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(CascConstants.CKeySize);
            return new CascKey(bytes);
        }

        /// <summary>
        /// Reads an encoded key.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <param name="length">The length of the key.</param>
        /// <returns>The encoded key.</returns>
        public static EKey ReadEKey(this BinaryReader reader, int length)
        {
            var bytes = reader.ReadBytes(length);
            return new EKey(bytes);
        }

        /// <summary>
        /// Reads a truncated encoded key.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The truncated encoded key.</returns>
        public static EKey ReadEKeyTruncated(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(CascConstants.EKeySize);
            return new EKey(bytes);
        }

        /// <summary>
        /// Reads an MD5 hash.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The MD5 hash.</returns>
        public static byte[] ReadMD5Hash(this BinaryReader reader)
        {
            return reader.ReadBytes(CascConstants.MD5HashSize);
        }

        /// <summary>
        /// Reads a SHA1 hash.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <returns>The SHA1 hash.</returns>
        public static byte[] ReadSHA1Hash(this BinaryReader reader)
        {
            return reader.ReadBytes(CascConstants.SHA1HashSize);
        }

        /// <summary>
        /// Skips a specified number of bytes.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <param name="count">The number of bytes to skip.</param>
        public static void Skip(this BinaryReader reader, int count)
        {
            if (reader.BaseStream.CanSeek)
            {
                reader.BaseStream.Seek(count, SeekOrigin.Current);
            }
            else
            {
                reader.ReadBytes(count);
            }
        }
    }
}