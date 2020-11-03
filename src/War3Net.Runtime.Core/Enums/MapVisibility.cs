// ------------------------------------------------------------------------------
// <copyright file="MapVisibility.cs" company="Drake53">
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
    public sealed class MapVisibility : Handle
    {
        private static readonly Dictionary<int, MapVisibility> _visibilities = GetTypes().ToDictionary(t => (int)t, t => new MapVisibility(t));

        private readonly Type _type;

        private MapVisibility(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static implicit operator Type(MapVisibility mapVisibility) => mapVisibility._type;

        public static explicit operator int(MapVisibility mapVisibility) => (int)mapVisibility._type;

        public static MapVisibility GetMapVisibility(int i)
        {
            if (!_visibilities.TryGetValue(i, out var mapVisibility))
            {
                mapVisibility = new MapVisibility((Type)i);
                _visibilities.Add(i, mapVisibility);
            }

            return mapVisibility;
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