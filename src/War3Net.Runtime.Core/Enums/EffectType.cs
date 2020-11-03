// ------------------------------------------------------------------------------
// <copyright file="EffectType.cs" company="Drake53">
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
    public sealed class EffectType : Handle
    {
        private static readonly Dictionary<int, EffectType> _types = GetTypes().ToDictionary(t => (int)t, t => new EffectType(t));

        private readonly Type _type;

        private EffectType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Effect = 0,
            Target = 1,
            Caster = 2,
            Special = 3,
            AreaEffect = 4,
            Missile = 5,
            Lightning = 6,
        }

        public static implicit operator Type(EffectType effectType) => effectType._type;

        public static explicit operator int(EffectType effectType) => (int)effectType._type;

        public static EffectType GetEffectType(int i)
        {
            if (!_types.TryGetValue(i, out var effectType))
            {
                effectType = new EffectType((Type)i);
                _types.Add(i, effectType);
            }

            return effectType;
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