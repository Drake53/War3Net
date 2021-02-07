// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace War3Net.Common.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadChars(this BinaryReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var baseStream = reader.BaseStream;
            var start = baseStream.Position;
            while (true)
            {
                if (baseStream.Position >= baseStream.Length)
                {
                    throw new InvalidDataException("Reached end of the stream without encountering a \0 character to mark the end of the string.");
                }

                if (reader.ReadByte() == char.MinValue)
                {
                    var bytesToRead = (int)(baseStream.Position - start - 1);
                    baseStream.Position = start;

                    var bytes = reader.ReadBytes(bytesToRead);
                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    // Read the \0 character.
                    reader.ReadByte();

                    return result;
                }
            }
        }

        public static string ReadString(this BinaryReader reader, int count)
        {
            return new string(reader.ReadChars(count)).TrimEnd(char.MinValue);
        }

        public static bool ReadBool(this BinaryReader reader)
        {
            return reader.ReadInt32().ToBool();
        }

        public static Color ReadColorRgba(this BinaryReader reader)
        {
            var red = reader.ReadByte();
            var green = reader.ReadByte();
            var blue = reader.ReadByte();
            var alpha = reader.ReadByte();
            return Color.FromArgb(alpha, red, green, blue);
        }

        public static int ReadInt24(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(3);
            var unsignedValue = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16);
            return unsignedValue >= 1 << 23 ? unsignedValue - (1 << 24) : unsignedValue;
        }

        public static uint ReadUInt24(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(3);
            return (uint)(bytes[0] | (bytes[1] << 8) | (bytes[2] << 16));
        }

        public static TEnum ReadByte<TEnum>(this BinaryReader reader)
            where TEnum : struct, Enum
        {
            return ToEnum<TEnum>(reader.ReadByte());
        }

        public static TEnum ReadChar<TEnum>(this BinaryReader reader)
            where TEnum : struct, Enum
        {
            return ToEnum<TEnum>(reader.ReadChar());
        }

        public static TEnum ReadInt32<TEnum>(this BinaryReader reader)
            where TEnum : struct, Enum
        {
            return ToEnum<TEnum>(reader.ReadInt32());
        }

        private static TEnum ToEnum<TEnum>(int i)
            where TEnum : struct, Enum
        {
            var result = (TEnum)(object)i;
            return result.IsDefined() ? result : throw new InvalidDataException($"Value '{i}' is not defined for enum of type {typeof(TEnum).Name}.");
        }
    }
}