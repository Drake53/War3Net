// ------------------------------------------------------------------------------
// <copyright file="UnitState.cs" company="Drake53">
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
    public sealed class UnitState : Handle
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

        public static implicit operator Type(UnitState unitState) => unitState._type;

        public static explicit operator int(UnitState unitState) => (int)unitState._type;

        public static UnitState GetUnitState(int i)
        {
            if (!_states.TryGetValue(i, out var unitState))
            {
                unitState = new UnitState((Type)i);
                _states.Add(i, unitState);
            }

            return unitState;
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