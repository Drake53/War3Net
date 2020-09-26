// ------------------------------------------------------------------------------
// <copyright file="ItemRealField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums.Object
{
    public sealed class ItemRealField
    {
        private static readonly Dictionary<int, ItemRealField> _fields = GetTypes().ToDictionary(t => (int)t, t => new ItemRealField(t));

        private readonly Type _type;

        private ItemRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            SCALING_VALUE = 1769169761,
        }

        public static ItemRealField GetItemRealField(int i)
        {
            if (!_fields.TryGetValue(i, out var itemRealField))
            {
                itemRealField = new ItemRealField((Type)i);
                _fields.Add(i, itemRealField);
            }

            return itemRealField;
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