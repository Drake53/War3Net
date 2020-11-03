// ------------------------------------------------------------------------------
// <copyright file="MapSetting.cs" company="Drake53">
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
    public sealed class MapSetting : Handle
    {
        private static readonly Dictionary<int, MapSetting> _settings = GetTypes().ToDictionary(t => (int)t, t => new MapSetting(t));

        private readonly Type _type;

        private MapSetting(Type type)
        {
            _type = type;
        }

        public enum Type
        {
        }

        public static implicit operator Type(MapSetting mapSetting) => mapSetting._type;

        public static explicit operator int(MapSetting mapSetting) => (int)mapSetting._type;

        public static MapSetting GetMapSetting(int i)
        {
            if (!_settings.TryGetValue(i, out var mapSetting))
            {
                mapSetting = new MapSetting((Type)i);
                _settings.Add(i, mapSetting);
            }

            return mapSetting;
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