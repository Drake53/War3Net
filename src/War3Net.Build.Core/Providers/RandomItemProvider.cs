// ------------------------------------------------------------------------------
// <copyright file="RandomItemProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Info;

namespace War3Net.Build.Providers
{
    public static class RandomItemProvider
    {
        // use -1 for 'any level'
        public static char[] GetRandomItemTypeCode(ItemClass itemClass, int level)
        {
            return new[] { 'Y', (char)itemClass, 'I', (char)(level + 48) };
        }

        public static bool IsRandomItem(char[] code, out int level, out int @class)
        {
            if (code[0] == 'Y' && code[2] == 'I' && Enum.IsDefined(typeof(ItemClass), (int)code[1]))
            {
                level = code[3] - 48;
                @class = (ItemClass)code[1] switch
                {
                    ItemClass.Any => 8,
                    ItemClass.Permanent => 0,
                    ItemClass.Charged => 1,
                    ItemClass.PowerUp => 2,
                    ItemClass.Artifact => 3,
                    ItemClass.Purchasable => 4,
                    ItemClass.Campaign => 5,
                    ItemClass.Miscellaneous => 6,

                    _ => 7,
                };

                return true;
            }

            level = default;
            @class = default;
            return false;
        }
    }
}