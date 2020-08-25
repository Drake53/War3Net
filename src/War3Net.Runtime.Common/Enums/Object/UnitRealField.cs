// ------------------------------------------------------------------------------
// <copyright file="UnitRealField.cs" company="Drake53">
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
    public sealed class UnitRealField
    {
        private static readonly Dictionary<int, UnitRealField> _fields = GetTypes().ToDictionary(t => (int)t, t => new UnitRealField(t));

        private readonly Type _type;

        private UnitRealField(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            STRENGTH_PER_LEVEL = 1970500720,
            AGILITY_PER_LEVEL = 1969317744,
            INTELLIGENCE_PER_LEVEL = 1969843824,
            HIT_POINTS_REGENERATION_RATE = 1969778802,
            MANA_REGENERATION = 1970106482,
            DEATH_TIME = 1969517677,
            FLY_HEIGHT = 1969650024,
            TURN_RATE = 1970108018,
            ELEVATION_SAMPLE_RADIUS = 1969582692,
            FOG_OF_WAR_SAMPLE_RADIUS = 1969648228,
            MAXIMUM_PITCH_ANGLE_DEGREES = 1970108528,
            MAXIMUM_ROLL_ANGLE_DEGREES = 1970108530,
            SCALING_VALUE = 1970496353,
            ANIMATION_RUN_SPEED = 1970435438,
            SELECTION_SCALE = 1970500451,
            SELECTION_CIRCLE_HEIGHT = 1970498682,
            SHADOW_IMAGE_HEIGHT = 1970497640,
            SHADOW_IMAGE_WIDTH = 1970497655,
            SHADOW_IMAGE_CENTER_X = 1970497656,
            SHADOW_IMAGE_CENTER_Y = 1970497657,
            ANIMATION_WALK_SPEED = 1970757996,
            DEFENSE = 1969514083,
            SIGHT_RADIUS = 1970497906,
            PRIORITY = 1970303593,
            SPEED = 1970108003,
            OCCLUDER_HEIGHT = 1970234211,
            HP = 1969778787,
            MANA = 1970106467,
            ACQUISITION_RANGE = 1969316721,
            CAST_BACK_SWING = 1969447539,
            CAST_POINT = 1969451124,
            MINIMUM_ATTACK_RANGE = 1969319278,
        }

        public static UnitRealField? GetUnitRealField(int i)
        {
            return _fields.TryGetValue(i, out var unitRealField) ? unitRealField : null;
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