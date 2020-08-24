// ------------------------------------------------------------------------------
// <copyright file="ItemIntegerField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class ItemIntegerField
    {
        private static readonly Dictionary<int, ItemIntegerField> _events = GetTypes().ToDictionary(t => (int)t, t => new ItemIntegerField(t));

        private readonly Type _type;

        private ItemIntegerField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            LEVEL = 1768711542,
            NUMBER_OF_CHARGES = 1769304933,
            COOLDOWN_GROUP = 1768122724,
            MAX_HIT_POINTS = 1768453232,
            HIT_POINTS = 1768452195,
            PRIORITY = 1768977001,
            ARMOR_TYPE = 1767993965,
            TINTING_COLOR_RED = 1768123506,
            TINTING_COLOR_GREEN = 1768123495,
            TINTING_COLOR_BLUE = 1768123490,
            TINTING_COLOR_ALPHA = 1768120684,
        }

        public static ItemIntegerField? GetItemIntegerField(int i)
        {
            return _events.TryGetValue(i, out var itemIntegerField) ? itemIntegerField : null;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}