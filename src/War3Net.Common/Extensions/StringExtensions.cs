// ------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Common.Extensions
{
    public static class StringExtensions
    {
        public static int FromRawcode(this string code)
        {
            return (code?.Length ?? 0) == 4
                ? code[0] | (code[1] << 8) | (code[2] << 16) | (code[3] << 24)
                : 0;
        }
    }
}