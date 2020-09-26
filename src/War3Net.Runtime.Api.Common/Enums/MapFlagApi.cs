// ------------------------------------------------------------------------------
// <copyright file="MapFlagApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class MapFlagApi
    {
        public static readonly MapFlag MAP_FOG_HIDE_TERRAIN = ConvertMapFlag((int)MapFlag.Type.FogHideTerrain);
        public static readonly MapFlag MAP_FOG_MAP_EXPLORED = ConvertMapFlag((int)MapFlag.Type.FogMapExplored);
        public static readonly MapFlag MAP_FOG_ALWAYS_VISIBLE = ConvertMapFlag((int)MapFlag.Type.FogAlwaysVisible);

        public static readonly MapFlag MAP_USE_HANDICAPS = ConvertMapFlag((int)MapFlag.Type.UseHandicaps);
        public static readonly MapFlag MAP_OBSERVERS = ConvertMapFlag((int)MapFlag.Type.Observers);
        public static readonly MapFlag MAP_OBSERVERS_ON_DEATH = ConvertMapFlag((int)MapFlag.Type.ObserversOnDeath);

        public static readonly MapFlag MAP_FIXED_COLORS = ConvertMapFlag((int)MapFlag.Type.FixedColors);

        public static readonly MapFlag MAP_LOCK_RESOURCE_TRADING = ConvertMapFlag((int)MapFlag.Type.LockResourceTrading);
        public static readonly MapFlag MAP_RESOURCE_TRADING_ALLIES_ONLY = ConvertMapFlag((int)MapFlag.Type.ResourceTradingAlliesOnly);

        public static readonly MapFlag MAP_LOCK_ALLIANCE_CHANGES = ConvertMapFlag((int)MapFlag.Type.LockAllianceChanges);
        public static readonly MapFlag MAP_ALLIANCE_CHANGES_HIDDEN = ConvertMapFlag((int)MapFlag.Type.AllianceChangesHidden);

        public static readonly MapFlag MAP_CHEATS = ConvertMapFlag((int)MapFlag.Type.Cheats);
        public static readonly MapFlag MAP_CHEATS_HIDDEN = ConvertMapFlag((int)MapFlag.Type.CheatsHidden);

        public static readonly MapFlag MAP_LOCK_SPEED = ConvertMapFlag((int)MapFlag.Type.LockSpeed);
        public static readonly MapFlag MAP_LOCK_RANDOM_SEED = ConvertMapFlag((int)MapFlag.Type.LockRandomSeed);
        public static readonly MapFlag MAP_SHARED_ADVANCED_CONTROL = ConvertMapFlag((int)MapFlag.Type.SharedAdvancedControl);
        public static readonly MapFlag MAP_RANDOM_HERO = ConvertMapFlag((int)MapFlag.Type.RandomHero);
        public static readonly MapFlag MAP_RANDOM_RACES = ConvertMapFlag((int)MapFlag.Type.RandomRaces);
        public static readonly MapFlag MAP_RELOADED = ConvertMapFlag((int)MapFlag.Type.Reloaded);

        public static MapFlag ConvertMapFlag(int i)
        {
            return MapFlag.GetMapFlag(i);
        }
    }
}