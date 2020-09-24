// ------------------------------------------------------------------------------
// <copyright file="MapControl.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class MapControl
    {
        private static readonly Dictionary<int, MapControl> _controls = GetTypes().ToDictionary(t => (int)t, t => new MapControl(t));

        private readonly Type _type;

        private MapControl(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            User = 0,
            Computer = 1,
            Rescuable = 2,
            Neutral = 3,
            Creep = 4,
            None = 5,
        }

        public static MapControl GetMapControl(int i)
        {
            if (!_controls.TryGetValue(i, out var mapControl))
            {
                mapControl = new MapControl((Type)i);
                _controls.Add(i, mapControl);
            }

            return mapControl;
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