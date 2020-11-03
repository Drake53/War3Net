// ------------------------------------------------------------------------------
// <copyright file="RarityControl.cs" company="Drake53">
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
    public sealed class RarityControl : Handle
    {
        private static readonly Dictionary<int, RarityControl> _controls = GetTypes().ToDictionary(t => (int)t, t => new RarityControl(t));

        private readonly Type _type;

        private RarityControl(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Frequent = 0,
            Rare = 1,
        }

        public static implicit operator Type(RarityControl rarityControl) => rarityControl._type;

        public static explicit operator int(RarityControl rarityControl) => (int)rarityControl._type;

        public static RarityControl GetRarityControl(int i)
        {
            if (!_controls.TryGetValue(i, out var rarityControl))
            {
                rarityControl = new RarityControl((Type)i);
                _controls.Add(i, rarityControl);
            }

            return rarityControl;
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