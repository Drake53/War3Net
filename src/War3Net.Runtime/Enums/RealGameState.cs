// ------------------------------------------------------------------------------
// <copyright file="RealGameState.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class RealGameState : GameState
    {
        private static readonly Dictionary<int, RealGameState> _states = GetTypes().ToDictionary(t => (int)t, t => new RealGameState(t));

        private readonly Type _type;

        private RealGameState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            TimeOfDay = 2,
        }

        public static RealGameState GetRealGameState(int i)
        {
            if (!_states.TryGetValue(i, out var realGameState))
            {
                realGameState = new RealGameState((Type)i);
                _states.Add(i, realGameState);
            }

            return realGameState;
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