// ------------------------------------------------------------------------------
// <copyright file="AnimationType.cs" company="Drake53">
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
    public sealed class AnimationType
    {
        private static readonly Dictionary<int, AnimationType> _types = GetTypes().ToDictionary(t => (int)t, t => new AnimationType(t));

        private readonly Type _type;

        private AnimationType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Birth = 0,
            Death = 1,
            Decay = 2,
            Dissipate = 3,
            Stand = 4,
            Walk = 5,
            Attack = 6,
            Morph = 7,
            Sleep = 8,
            Spell = 9,
            Portrait = 10,
        }

        public static AnimationType GetAnimationType(int i)
        {
            if (!_types.TryGetValue(i, out var animationType))
            {
                animationType = new AnimationType((Type)i);
                _types.Add(i, animationType);
            }

            return animationType;
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