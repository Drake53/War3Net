// ------------------------------------------------------------------------------
// <copyright file="MapVisibility.cs" company="Drake53">
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
    public sealed class MapVisibility
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