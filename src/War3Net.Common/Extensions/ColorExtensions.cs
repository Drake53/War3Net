// ------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;

namespace War3Net.Common.Extensions
{
    public static class ColorExtensions
    {
        public static int ToRgba(this Color color)
        {
            return (color.A << 24) | (color.B << 16) | (color.G << 8) | color.R;
        }

        public static int ToBgra(this Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }
    }
}