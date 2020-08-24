// ------------------------------------------------------------------------------
// <copyright file="UnitStringField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class UnitStringField
    {
        private static readonly Dictionary<int, UnitStringField> _events = GetTypes().ToDictionary(t => (int)t, t => new UnitStringField(t));

        private readonly Type _type;

        private UnitStringField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            NAME = 1970168173,
            PROPER_NAMES = 1970303599,
            GROUND_TEXTURE = 1970627187,
            SHADOW_IMAGE_UNIT = 1970497653,
        }

        public static UnitStringField? GetUnitStringField(int i)
        {
            return _events.TryGetValue(i, out var unitStringField) ? unitStringField : null;
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