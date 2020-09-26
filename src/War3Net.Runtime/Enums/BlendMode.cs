// ------------------------------------------------------------------------------
// <copyright file="BlendMode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class BlendMode
    {
        private static readonly Dictionary<int, BlendMode> _modes = GetTypes().ToDictionary(t => (int)t, t => new BlendMode(t));

        private readonly Type _type;

        private BlendMode(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            None = 0,
            Alpha = 1,
            Blend = 2,
            Additive = 3,
            Modulate = 4,
            Modulate2x = 5,
        }

        public static BlendMode GetBlendMode(int i)
        {
            if (!_modes.TryGetValue(i, out var blendMode))
            {
                blendMode = new BlendMode((Type)i);
                _modes.Add(i, blendMode);
            }

            return blendMode;
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