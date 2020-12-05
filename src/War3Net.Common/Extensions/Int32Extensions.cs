// ------------------------------------------------------------------------------
// <copyright file="Int32Extensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;

namespace War3Net.Common.Extensions
{
    public static class Int32Extensions
    {
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

        public static Color ToRgbaColor(this int i)
        {
            return Color.FromArgb(
                (int)((i & 0xFF000000) >> 24),
                i & 0x000000FF,
                (i & 0x0000FF00) >> 8,
                (i & 0x00FF0000) >> 16);
        }
    }
}