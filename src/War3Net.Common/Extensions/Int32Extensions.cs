// ------------------------------------------------------------------------------
// <copyright file="Int32Extensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace War3Net.Common.Extensions
{
    public static class Int32Extensions
    {
        public static bool ToBool(this int value)
        {
            return value switch
            {
                0 => false,
                1 => true,
                _ => throw new ArgumentException($"A 32-bit bool must be either 0 or 1, but got '{value}'."),
            };
        }

        public static string ToRawcode(this int value)
        {
            return new string(new[]
            {
                (char)(value & 0x000000FF),
                (char)((value & 0x0000FF00) >> 8),
                (char)((value & 0x00FF0000) >> 16),
                (char)((value & 0xFF000000) >> 24),
            });
        }

        public static Color ToRgbaColor(this int value)
        {
            return Color.FromArgb(
                (int)((value & 0xFF000000) >> 24),
                value & 0x000000FF,
                (value & 0x0000FF00) >> 8,
                (value & 0x00FF0000) >> 16);
        }
    }
}