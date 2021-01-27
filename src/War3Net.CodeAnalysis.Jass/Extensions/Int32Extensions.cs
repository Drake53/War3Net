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
            for (var i = 1; i < 4; i++)
            {
                if (bytes[i] >= 0x80)
                {
                    bytes[i - 1]++;
                }
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}