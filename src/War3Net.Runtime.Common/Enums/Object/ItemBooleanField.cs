// ------------------------------------------------------------------------------
// <copyright file="ItemBooleanField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class ItemBooleanField
    {
        private static readonly Dictionary<int, ItemBooleanField> _fields = GetTypes().ToDictionary(t => (int)t, t => new ItemBooleanField(t));

        private readonly Type _type;

        private ItemBooleanField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            DROPPED_WHEN_CARRIER_DIES = 1768190576,
            CAN_BE_DROPPED = 1768190575,
            PERISHABLE = 1768973682,
            INCLUDE_AS_RANDOM_CHOICE = 1768977006,
            USE_AUTOMATICALLY_WHEN_ACQUIRED = 1768976247,
            CAN_BE_SOLD_TO_MERCHANTS = 1768972663,
            ACTIVELY_USED = 1769304929,
        }

        public static ItemBooleanField GetItemBooleanField(int i)
        {
            if (!_fields.TryGetValue(i, out var itemBooleanField))
            {
                itemBooleanField = new ItemBooleanField((Type)i);
                _fields.Add(i, itemBooleanField);
            }

            return itemBooleanField;
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