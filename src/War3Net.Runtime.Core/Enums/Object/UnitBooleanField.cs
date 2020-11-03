// ------------------------------------------------------------------------------
// <copyright file="UnitBooleanField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums.Object
{
    public sealed class UnitBooleanField : Handle
    {
        private static readonly Dictionary<int, UnitBooleanField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitBooleanField(t));

        private readonly Type _type;

        private UnitBooleanField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            RAISABLE = 1970430313,
            DECAYABLE = 1969513827,
            IS_A_BUILDING = 1969382503,
            USE_EXTENDED_LINE_OF_SIGHT = 1970040691,
            NEUTRAL_BUILDING_SHOWS_MINIMAP_ICON = 1970168429,
            HERO_HIDE_HERO_INTERFACE_ICON = 1969776738,
            HERO_HIDE_HERO_MINIMAP_DISPLAY = 1969776749,
            HERO_HIDE_HERO_DEATH_MESSAGE = 1969776740,
            HIDE_MINIMAP_DISPLAY = 1969778541,
            SCALE_PROJECTILES = 1970496354,
            SELECTION_CIRCLE_ON_WATER = 1970496887,
            HAS_WATER_SHADOW = 1970497650,
        }

        public static implicit operator Type(UnitBooleanField unitBooleanField) => unitBooleanField._type;

        public static explicit operator int(UnitBooleanField unitBooleanField) => (int)unitBooleanField._type;

        public static UnitBooleanField GetUnitBooleanField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitBooleanField))
            {
                unitBooleanField = new UnitBooleanField((Type)i);
                _fields.Add(i, unitBooleanField);
            }

            return unitBooleanField;
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