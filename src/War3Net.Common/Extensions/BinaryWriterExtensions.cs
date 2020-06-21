// ------------------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;

namespace War3Net.Common.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteString(this BinaryWriter writer, string? s)
        {
            var endsWithNullChar = false;
            foreach (var c in s ?? string.Empty)
            {
                if (endsWithNullChar)
                {
                    throw new ArgumentException("String is not allowed to contain \0, unless it is the last character.", nameof(s));
                }

                writer.Write(c);
                endsWithNullChar = c == char.MinValue;
            }

            if (!endsWithNullChar)
            {
                writer.Write(char.MinValue);
            }
        }

        public static void WriteString(this BinaryWriter writer, string? s, int length)
        {
            s ??= string.Empty;

            var stringLength = s.Length;
            if (stringLength > length)
            {
                throw new ArgumentOutOfRangeException(nameof(s));
            }

            if (stringLength < length)
            {
                // Add padding
                s = new string(s.Concat(new string(char.MinValue, length - stringLength)).ToArray());
            }

            foreach (var c in s)
            {
                writer.Write(c);
            }
        }
    }
}