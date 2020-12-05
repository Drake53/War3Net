// ------------------------------------------------------------------------------
// <copyright file="RandomUnitProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Common.Extensions;

namespace War3Net.Build.Providers
{
    public static class RandomUnitProvider
    {
        // use -1 for 'any level'
        public static int GetRandomUnitTypeCode(int level)
        {
            return $"YYU{(char)('0' + level)}".FromRawcode();
        }

        public static bool IsRandomUnit(int code, out int level)
        {
            var codeString = code.ToRawcode();
            if (codeString[0] == 'Y' && codeString[1] == 'Y' && codeString[2] == 'U')
            {
                level = codeString[3] - '0';
                return true;
            }

            level = default;
            return false;
        }
    }
}