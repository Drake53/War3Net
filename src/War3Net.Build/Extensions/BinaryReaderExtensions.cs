// ------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

namespace War3Net.Build.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadChars(this BinaryReader reader)
        {
            // todo: use stringbuilder?
            var s = string.Empty;
            while (true)
            {
                var read = reader.ReadChar();
                if (read == char.MinValue)
                {
                    break;
                }

                s += read;
            }

            return s;
        }

        public static Color ReadColorRgba(this BinaryReader reader)
        {
            var red = reader.ReadByte();
            var green = reader.ReadByte();
            var blue = reader.ReadByte();
            var alpha = reader.ReadByte();
            return Color.FromArgb(alpha, red, green, blue);
        }
    }
}