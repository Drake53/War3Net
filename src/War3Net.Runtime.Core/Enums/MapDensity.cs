using System;
using System.Collections.Generic;
using System.Linq;
using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class MapDensity : Handle
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

        public static implicit operator Type(MapDensity mapDensity) => mapDensity._type;

        public static explicit operator int(MapDensity mapDensity) => (int)mapDensity._type;

        public static MapDensity GetMapDensity(int i)
        {
            if (!_densities.TryGetValue(i, out var mapDensity))
            {
                mapDensity = new MapDensity((Type)i);
                _densities.Add(i, mapDensity);
            }

            return mapDensity;
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