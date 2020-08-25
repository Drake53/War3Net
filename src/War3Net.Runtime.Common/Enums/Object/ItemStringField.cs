// ------------------------------------------------------------------------------
// <copyright file="ItemStringField.cs" company="Drake53">
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

        public static ItemStringField? GetItemStringField(int i)
        {
            return _fields.TryGetValue(i, out var itemStringField) ? itemStringField : null;
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