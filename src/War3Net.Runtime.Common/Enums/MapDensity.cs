// ------------------------------------------------------------------------------
// <copyright file="MapDensity.cs" company="Drake53">
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
    public sealed class MapDensity
    {
        private static readonly Dictionary<int, MapDensity> _densities = GetTypes().ToDictionary(t => (int)t, t => new MapDensity(t));

        private readonly Type _type;

        private MapDensity(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            None = 0,
            Light = 1,
            Medium = 2,
            Heavy = 3,
        }

        public static MapDensity? GetMapDensity(int i)
        {
            return _densities.TryGetValue(i, out var mapDensity) ? mapDensity : null;
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