// ------------------------------------------------------------------------------
// <copyright file="RandomItemProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Providers
{
    public static class RandomItemProvider
    {
        // use -1 for 'any level'
        public static int GetRandomItemTypeCode(ItemClass itemClass, int level)
        {
            return $"Y{(itemClass == ItemClass.Any ? 'Y' : (char)('i' + itemClass))}I{(char)('0' + level)}".FromRawcode();
        }

        public static bool IsRandomItem(int code, out ItemClass itemClass, out int level)
        {
            var codeString = code.ToRawcode();
            if (codeString[0] == 'Y' && codeString[2] == 'I' && Enum.IsDefined(typeof(ItemClass), codeString[1] - 'i'))
            {
                itemClass = codeString[1] == 'Y' ? ItemClass.Any : (ItemClass)(codeString[1] - 'i');
                level = codeString[3] - '0';
                return true;
            }

            itemClass = default;
            level = default;
            return false;
        }
    }
}