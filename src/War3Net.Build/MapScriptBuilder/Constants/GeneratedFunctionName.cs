// ------------------------------------------------------------------------------
// <copyright file="GeneratedFunctionName.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        internal static class GeneratedFunctionName
        {
            internal const string Config = "config";
            internal const string CreateAllDestructables = "CreateAllDestructables";
            internal const string CreateAllItems = "CreateAllItems";
            internal const string CreateAllUnits = "CreateAllUnits";
            internal const string CreateCameras = "CreateCameras";
            internal const string CreateNeutralHostile = "CreateNeutralHostile";
            internal const string CreateNeutralHostileBuildings = "CreateNeutralHostileBuildings";
            internal const string CreateNeutralPassive = "CreateNeutralPassive";
            internal const string CreateNeutralPassiveBuildings = "CreateNeutralPassiveBuildings";
            internal const string CreateNeutralUnits = "CreateNeutralUnits";
            internal const string CreatePlayerBuildings = "CreatePlayerBuildings";
            internal const string CreatePlayerUnits = "CreatePlayerUnits";
            internal const string CreateRegions = "CreateRegions";
            internal const string InitAllyPriorities = "InitAllyPriorities";
            internal const string InitCustomPlayerSlots = "InitCustomPlayerSlots";
            internal const string InitCustomTeams = "InitCustomTeams";
            internal const string InitCustomTriggers = "InitCustomTriggers";
            internal const string InitGlobals = "InitGlobals";
            internal const string InitRandomGroups = "InitRandomGroups";
            internal const string InitSounds = "InitSounds";
            internal const string InitTechTree = "InitTechTree";
            internal const string InitUpgrades = "InitUpgrades";
            internal const string Main = "main";
            internal const string RunInitializationTriggers = "RunInitializationTriggers";

            internal static string CreateBuildingsForPlayer(int playerId) => $"CreateBuildingsForPlayer{playerId}";

            internal static string CreateUnitsForPlayer(int playerId) => $"CreateUnitsForPlayer{playerId}";

            internal static string InitTechTreeForPlayer(int playerId) => $"InitTechTree_Player{playerId}";

            internal static string InitUpgradesForPlayer(int playerId) => $"InitUpgrades_Player{playerId}";
        }
    }
}