// ------------------------------------------------------------------------------
// <copyright file="Int32Extensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class Int32Extensions
    {
        public static string ToJassRawcode(this int value)
        {
            var bytes = BitConverter.GetBytes(value).Reverse().ToArray();
            for (var i = 3; i > 0; i--)
            {
                if (bytes[i] >= 0x80)
                {
                    bytes[i - 1]++;
                }
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public static int InvertEndianness(this int value)
        {
            return
                (value & unchecked((int)0xFF000000)) >> 24 |
                (value & 0x00FF0000) >> 8 |
                (value & 0x0000FF00) << 8 |
                (value & 0x000000FF) << 24;
        }
    }
}