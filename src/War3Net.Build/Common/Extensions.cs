// ------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Common
{
    public static class Extensions
    {
        public static int FromRawcode(this string code)
        {
            return (code?.Length ?? 0) == 4
                ? code[0] | (code[1] << 8) | (code[2] << 16) | (code[3] << 24)
                : 0;
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
    }
}