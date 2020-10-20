// ------------------------------------------------------------------------------
// <copyright file="PlayerApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Runtime.Enums;

using Player_ = War3Net.Runtime.Core.Player;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class PlayerApi
    {
        public static Player_ Player(int number) => Player_.GetPlayer(number);

        public static Player_ GetLocalPlayer() => Player_.LocalPlayer;

        public static bool IsPlayerAlly(Player_? whichPlayer, Player_? otherPlayer) => throw new NotImplementedException();

        public static bool IsPlayerEnemy(Player_? whichPlayer, Player_? otherPlayer) => throw new NotImplementedException();

        // public static bool IsPlayerInForce(Player_? whichPlayer, Force? whichForce) => throw new NotImplementedException();

        public static bool IsPlayerObserver(Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsVisibleToPlayer(float x, float y, Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsLocationVisibleToPlayer(Location? whichLocation, Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsFoggedToPlayer(float x, float y, Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsLocationFoggedToPlayer(Location? whichLocation, Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsMaskedToPlayer(float x, float y, Player_? whichPlayer) => throw new NotImplementedException();

        public static bool IsLocationMaskedToPlayer(Location? whichLocation, Player_? whichPlayer) => throw new NotImplementedException();

        public static Race GetPlayerRace(Player_? whichPlayer) => throw new NotImplementedException();

        public static int GetPlayerId(Player_? whichPlayer) => whichPlayer.Id;

        public static int GetPlayerUnitCount(Player_? whichPlayer, bool includeIncomplete) => throw new NotImplementedException();

        public static int GetPlayerTypedUnitCount(Player_? whichPlayer, string? unitName, bool includeIncomplete, bool includeUpgrades) => throw new NotImplementedException();

        public static int GetPlayerStructureCount(Player_? whichPlayer, bool includeIncomplete) => throw new NotImplementedException();

        public static int GetPlayerState(Player_? whichPlayer, PlayerState? whichPlayerState) => throw new NotImplementedException();

        public static int GetPlayerScore(Player_? whichPlayer, PlayerScore? whichPlayerScore) => throw new NotImplementedException();

        public static bool GetPlayerAlliance(Player_? sourcePlayer, Player_? otherPlayer, AllianceType? whichAllianceSetting) => throw new NotImplementedException();

        public static float GetPlayerHandicap(Player_? whichPlayer) => throw new NotImplementedException();

        public static float GetPlayerHandicapXP(Player_? whichPlayer) => throw new NotImplementedException();

        public static void SetPlayerHandicap(Player_? whichPlayer, float handicap) => throw new NotImplementedException();

        public static void SetPlayerHandicapXP(Player_? whichPlayer, float handicap) => throw new NotImplementedException();

        public static void SetPlayerTechMaxAllowed(Player_? whichPlayer, int techid, int maximum) => throw new NotImplementedException();

        public static int GetPlayerTechMaxAllowed(Player_? whichPlayer, int techid) => throw new NotImplementedException();

        public static void AddPlayerTechResearched(Player_? whichPlayer, int techid, int levels) => throw new NotImplementedException();

        public static void SetPlayerTechResearched(Player_? whichPlayer, int techid, int setToLevel) => throw new NotImplementedException();

        public static bool GetPlayerTechResearched(Player_? whichPlayer, int techid, bool specificonly) => throw new NotImplementedException();

        public static int GetPlayerTechCount(Player_? whichPlayer, int techid, bool specificonly) => throw new NotImplementedException();

        public static void SetPlayerUnitsOwner(Player_? whichPlayer, int newOwner) => throw new NotImplementedException();

        // public static void CripplePlayer(Player_? whichPlayer, Force toWhichPlayers, bool flag) => throw new NotImplementedException();

        public static void SetPlayerAbilityAvailable(Player_? whichPlayer, int abilid, bool avail) => throw new NotImplementedException();

        public static void SetPlayerState(Player_? whichPlayer, PlayerState? whichPlayerState, int value) => throw new NotImplementedException();

        public static void RemovePlayer(Player_? whichPlayer, PlayerGameResult? gameResult) => throw new NotImplementedException();

        public static void CachePlayerHeroData(Player_? whichPlayer) => throw new NotImplementedException();

        public static int GetBJMaxPlayers() => Player_.MaxPlayerSlotCount;

        public static int GetBJPlayerNeutralVictim() => 0;

        public static int GetBJPlayerNeutralExtra() => 0;

        public static int GetBJMaxPlayerSlots() => Player_.MaxPlayerSlotCount;

        public static int GetPlayerNeutralPassive() => 24;

        public static int GetPlayerNeutralAggressive() => 27;
    }
}