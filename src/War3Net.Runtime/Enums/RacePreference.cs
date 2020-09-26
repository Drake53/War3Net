// ------------------------------------------------------------------------------
// <copyright file="RacePreference.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Enums
{
    public sealed class RacePreference
    {
        private static readonly Dictionary<int, RacePreference> _preferences = GetTypes().ToDictionary(t => (int)t, t => new RacePreference(t));

        private readonly Type _type;

        private RacePreference(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            Human = 1 << 0,
            Orc = 1 << 1,
            NightElf = 1 << 2,
            Undead = 1 << 3,
            Demon = 1 << 4,
            Random = 1 << 5,
            UserSelectable = 1 << 6,
        }

        public static RacePreference GetRacePreference(int i)
        {
            if (!_preferences.TryGetValue(i, out var racePreference))
            {
                racePreference = new RacePreference((Type)i);
                _preferences.Add(i, racePreference);
            }

            return racePreference;
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