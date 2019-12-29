// ------------------------------------------------------------------------------
// <copyright file="RandomUnitProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Providers
{
    public static class RandomUnitProvider
    {
        // use -1 for 'any level'
        public static char[] GetRandomUnitTypeCode(int level)
        {
            return new[] { 'Y', 'Y', 'U', (char)(level + 48) };
        }

        public static bool IsRandomUnit(char[] code, out int level)
        {
            if (code[0] == 'Y' && code[1] == 'Y' && code[2] == 'U')
            {
                level = code[3] - 48;
                return true;
            }

            level = default;
            return false;
        }
    }
}