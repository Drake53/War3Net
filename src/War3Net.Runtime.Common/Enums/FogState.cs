// ------------------------------------------------------------------------------
// <copyright file="FogState.cs" company="Drake53">
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
    public sealed class FogState
    {
        private static readonly Dictionary<int, FogState> _states = GetTypes().ToDictionary(t => (int)t, t => new FogState(t));

        private readonly Type _type;

        private FogState(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Masked = 1 << 0,
            Fogged = 1 << 1,
            Visible = 1 << 2,
        }

        public static FogState? GetFogState(int i)
        {
            return _states.TryGetValue(i, out var fogState) ? fogState : null;
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