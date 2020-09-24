// ------------------------------------------------------------------------------
// <copyright file="UnitIntegerField.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums.Object
{
    public sealed class UnitIntegerField
    {
        private static readonly Dictionary<int, UnitIntegerField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitIntegerField(t));

        private readonly Type _type;

        private UnitIntegerField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            DEFENSE_TYPE = 1969517689,
            ARMOR_TYPE = 1969320557,
            LOOPING_FADE_IN_RATE = 1970038377,
            LOOPING_FADE_OUT_RATE = 1970038383,
            AGILITY = 1969317731,
            INTELLIGENCE = 1969843811,
            STRENGTH = 1970500707,
            AGILITY_PERMANENT = 1969317741,
            INTELLIGENCE_PERMANENT = 1969843821,
            STRENGTH_PERMANENT = 1970500717,
            AGILITY_WITH_BONUS = 1969317730,
            INTELLIGENCE_WITH_BONUS = 1969843810,
            STRENGTH_WITH_BONUS = 1970500706,
            GOLD_BOUNTY_AWARDED_NUMBER_OF_DICE = 1969382505,
            GOLD_BOUNTY_AWARDED_BASE = 1969381985,
            GOLD_BOUNTY_AWARDED_SIDES_PER_DIE = 1969386345,
            LUMBER_BOUNTY_AWARDED_NUMBER_OF_DICE = 1970037348,
            LUMBER_BOUNTY_AWARDED_BASE = 1970037345,
            LUMBER_BOUNTY_AWARDED_SIDES_PER_DIE = 1970037363,
            LEVEL = 1970038134,
            FORMATION_RANK = 1969647474,
            ORIENTATION_INTERPOLATION = 1970238057,
            ELEVATION_SAMPLE_POINTS = 1969582196,
            TINTING_COLOR_RED = 1969450098,
            TINTING_COLOR_GREEN = 1969450087,
            TINTING_COLOR_BLUE = 1969450082,
            TINTING_COLOR_ALPHA = 1969447276,
            MOVE_TYPE = 1970108020,
            TARGETED_AS = 1970561394,
            UNIT_CLASSIFICATION = 1970567536,
            HIT_POINTS_REGENERATION_TYPE = 1969779316,
            PLACEMENT_PREVENTED_BY = 1970299250,
            PRIMARY_ATTRIBUTE = 1970303585,
        }

        public static UnitIntegerField GetUnitIntegerField(int i)
        {
            if (!_fields.TryGetValue(i, out var unitIntegerField))
            {
                unitIntegerField = new UnitIntegerField((Type)i);
                _fields.Add(i, unitIntegerField);
            }

            return unitIntegerField;
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