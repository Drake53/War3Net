// ------------------------------------------------------------------------------
// <copyright file="UnitState.cs" company="Drake53">
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
    public sealed class UnitState
    {
        private static readonly Dictionary<int, UnitState> _states = GetTypes().ToDictionary(t => (int)t, t => new UnitState(t));

        private readonly Type _type;

        private UnitState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Life = 0,
            MaxLife = 1,
            Mana = 2,
            MaxMana = 3,
        }

        public static UnitState? GetUnitState(int i)
        {
            return _states.TryGetValue(i, out var unitState) ? unitState : null;
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