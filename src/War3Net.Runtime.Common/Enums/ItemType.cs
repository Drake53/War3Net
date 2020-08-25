// ------------------------------------------------------------------------------
// <copyright file="ItemType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class ItemType
    {
        private static readonly Dictionary<int, ItemType> _types = GetTypes().ToDictionary(t => (int)t, t => new ItemType(t));

        private readonly Type _type;

        private ItemType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Permanent = 0,
            Charged = 1,
            Powerup = 2,
            Artifact = 3,
            Purchasable = 4,
            Campaign = 5,
            Miscellaneous = 6,
            Unknown = 7,
            Any = 8,
        }

        public static ItemType? GetItemType(int i)
        {
            return _types.TryGetValue(i, out var itemType) ? itemType : null;
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