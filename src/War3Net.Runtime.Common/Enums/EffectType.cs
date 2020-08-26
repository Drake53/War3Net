// ------------------------------------------------------------------------------
// <copyright file="EffectType.cs" company="Drake53">
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
    public sealed class EffectType
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