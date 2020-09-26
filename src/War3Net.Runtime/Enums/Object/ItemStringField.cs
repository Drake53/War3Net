// ------------------------------------------------------------------------------
// <copyright file="ItemStringField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums.Object
{
    public sealed class ItemStringField
    {
        private static readonly Dictionary<int, ItemStringField> _fields = GetTypes().ToDictionary(t => (int)t, t => new ItemStringField(t));

        private readonly Type _type;

        private ItemStringField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            MODEL_USED = 1768319340,
        }

        public static ItemStringField GetItemStringField(int i)
        {
            if (!_fields.TryGetValue(i, out var itemStringField))
            {
                itemStringField = new ItemStringField((Type)i);
                _fields.Add(i, itemStringField);
            }

            return itemStringField;
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