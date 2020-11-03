// ------------------------------------------------------------------------------
// <copyright file="MouseButtonType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class MouseButtonType : Handle
    {
        private static readonly Dictionary<int, MouseButtonType> _types = GetTypes().ToDictionary(t => (int)t, t => new MouseButtonType(t));

        private readonly Type _type;

        private MouseButtonType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Left = 1,
            Middle = 2,
            Right = 3,
        }

        public static implicit operator Type(MouseButtonType mouseButtonType) => mouseButtonType._type;

        public static explicit operator int(MouseButtonType mouseButtonType) => (int)mouseButtonType._type;

        public static MouseButtonType GetMouseButtonType(int i)
        {
            if (!_types.TryGetValue(i, out var mouseButtonType))
            {
                mouseButtonType = new MouseButtonType((Type)i);
                _types.Add(i, mouseButtonType);
            }

            return mouseButtonType;
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