// ------------------------------------------------------------------------------
// <copyright file="AIDifficulty.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class AIDifficulty
    {
        private static readonly Dictionary<int, AIDifficulty> _difficulties = GetTypes().ToDictionary(t => (int)t, t => new AIDifficulty(t));

        private readonly Type _type;

        private AIDifficulty(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Newbie = 0,
            Normal = 1,
            Insane = 2,
        }

        public static AIDifficulty GetAIDifficulty(int i)
        {
            if (!_difficulties.TryGetValue(i, out var aiDifficulty))
            {
                aiDifficulty = new AIDifficulty((Type)i);
                _difficulties.Add(i, aiDifficulty);
            }

            return aiDifficulty;
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