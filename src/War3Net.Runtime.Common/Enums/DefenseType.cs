// ------------------------------------------------------------------------------
// <copyright file="DefenseType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class DefenseType
    {
        private static readonly Dictionary<int, DefenseType> _types = GetTypes().ToDictionary(t => (int)t, t => new DefenseType(t));

        private readonly Type _type;

        private DefenseType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Light = 0,
            Medium = 1,
            Large = 2,
            Fortified = 3,
            Normal = 4,
            Hero = 5,
            Divine = 6,
            None = 7,
        }

        public static DefenseType GetDefenseType(int i)
        {
            if (!_types.TryGetValue(i, out var defenseType))
            {
                defenseType = new DefenseType((Type)i);
                _types.Add(i, defenseType);
            }

            return defenseType;
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