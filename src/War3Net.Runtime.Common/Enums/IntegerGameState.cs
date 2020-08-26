// ------------------------------------------------------------------------------
// <copyright file="IntegerGameState.cs" company="Drake53">
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
    public sealed class IntegerGameState : GameState
    {
        private static readonly Dictionary<int, IntegerGameState> _states = GetTypes().ToDictionary(t => (int)t, t => new IntegerGameState(t));

        private readonly Type _type;

        private IntegerGameState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            DivineIntervention = 0,
            Disconnected = 1,
        }

        public static IntegerGameState GetIntegerGameState(int i)
        {
            if (!_states.TryGetValue(i, out var integerGameState))
            {
                integerGameState = new IntegerGameState((Type)i);
                _states.Add(i, integerGameState);
            }

            return integerGameState;
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