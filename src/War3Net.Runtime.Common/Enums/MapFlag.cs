// ------------------------------------------------------------------------------
// <copyright file="MapFlag.cs" company="Drake53">
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
    public sealed class MapFlag
    {
        private static readonly Dictionary<int, MapFlag> _flags = GetTypes().ToDictionary(t => (int)t, t => new MapFlag(t));

        private readonly Type _type;

        private MapFlag(Type type)
        {
            _type = type;
        }

        [Flags]
        public enum Type
        {
            FogHideTerrain = 1 << 0,
            FogMapExplored = 1 << 1,
            FogAlwaysVisible = 1 << 2,

            UseHandicaps = 1 << 3,
            Observers = 1 << 4,
            ObserversOnDeath = 1 << 5,

            FixedColors = 1 << 7,

            LockResourceTrading = 1 << 8,
            ResourceTradingAlliesOnly = 1 << 9,

            LockAllianceChanges = 1 << 10,
            AllianceChangesHidden = 1 << 11,

            Cheats = 1 << 12,
            CheatsHidden = 1 << 13,

            LockSpeed = 1 << 14,
            LockRandomSeed = 1 << 15,

            SharedAdvancedControl = 1 << 16,

            RandomHero = 1 << 17,
            RandomRaces = 1 << 18,

            Reloaded = 1 << 19,
        }

        public static MapFlag? GetMapFlag(int i)
        {
            return _flags.TryGetValue(i, out var mapFlag) ? mapFlag : null;
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