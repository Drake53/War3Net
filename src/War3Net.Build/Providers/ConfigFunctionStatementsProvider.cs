// ------------------------------------------------------------------------------
// <copyright file="ConfigFunctionStatementsProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Info;
using War3Net.Build.Script;

namespace War3Net.Build.Providers
{
    internal static class ConfigFunctionStatementsProvider<TBuilder, TStatementSyntax, TFunctionSyntax>
        where TBuilder : FunctionBuilder<TStatementSyntax, TFunctionSyntax>, IConfigFunctionBuilder<TStatementSyntax>
    {
        private const string ConfigFunctionName = "config";

        public static string GetConfigFunctionName => ConfigFunctionName;

        public static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.MapInfo;

            var playerDataCount = mapInfo.PlayerDataCount;
            var forceDataCount = mapInfo.ForceDataCount;

            yield return builder.GenerateInvocationStatementWithStringArgument(
                nameof(War3Api.Common.SetMapName),
                mapInfo.MapName);

            yield return builder.GenerateInvocationStatementWithStringArgument(
                nameof(War3Api.Common.SetMapDescription),
                mapInfo.MapDescription);

            yield return builder.GenerateInvocationStatementWithIntegerArgument(
                nameof(War3Api.Common.SetPlayers),
                playerDataCount);

            // Intuitively this would use forceDataCount, but so far all examples generated with World Editor have same argument for SetPlayers and SetTeams.
            yield return builder.GenerateInvocationStatementWithIntegerArgument(
                nameof(War3Api.Common.SetTeams),
                playerDataCount);

            yield return builder.GenerateInvocationStatementWithVariableArgument(
                nameof(War3Api.Common.SetGamePlacement),
                nameof(War3Api.Common.MAP_PLACEMENT_TEAMS_TOGETHER));

            if (builder.LobbyMusic != null)
            {
                yield return builder.GenerateInvocationStatementWithStringArgument(
                    nameof(War3Api.Common.PlayMusic),
                    builder.LobbyMusic);
            }

            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.GetPlayerData(i);

                yield return builder.GenerateDefineStartLocationStatement(
                    nameof(War3Api.Common.DefineStartLocation),
                    i,
                    playerData.StartPosition.X,
                    playerData.StartPosition.Y);
            }

            // InitCustomPlayerSlots
            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.GetPlayerData(i);

                yield return builder.GenerateSetPlayerStartLocationStatement(
                    nameof(War3Api.Common.SetPlayerStartLocation),
                    nameof(War3Api.Common.Player),
                    playerData.PlayerNumber,
                    i);

                if (playerData.FixedStartPosition)
                {
                    yield return builder.GenerateSetPlayerStartLocationStatement(
                        nameof(War3Api.Common.ForcePlayerStartLocation),
                        nameof(War3Api.Common.Player),
                        playerData.PlayerNumber,
                        i);
                }

                yield return builder.GenerateSetPlayerColorStatement(
                    nameof(War3Api.Common.SetPlayerColor),
                    nameof(War3Api.Common.Player),
                    nameof(War3Api.Common.ConvertPlayerColor),
                    playerData.PlayerNumber);

                yield return builder.GenerateSetPlayerPropertyToVariableStatement(
                    nameof(War3Api.Common.SetPlayerRacePreference),
                    nameof(War3Api.Common.Player),
                    playerData.PlayerNumber,
                    RacePreferenceProvider.GetRacePreferenceString(playerData.PlayerRace));

                yield return builder.GenerateSetPlayerRaceSelectableStatement(
                    nameof(War3Api.Common.SetPlayerRaceSelectable),
                    nameof(War3Api.Common.Player),
                    playerData.PlayerNumber,
                    playerData.IsRaceSelectable || !mapInfo.MapFlags.HasFlag(MapFlags.FixedPlayerSettingsForCustomForces));

                yield return builder.GenerateSetPlayerPropertyToVariableStatement(
                    nameof(War3Api.Common.SetPlayerController),
                    nameof(War3Api.Common.Player),
                    playerData.PlayerNumber,
                    PlayerControllerProvider.GetPlayerControllerString(playerData.PlayerController));

                if (playerData.PlayerController == PlayerController.Rescuable)
                {
                    for (var j = 0; j < playerDataCount; j++)
                    {
                        var otherPlayerData = mapInfo.GetPlayerData(j);
                        if (otherPlayerData.PlayerController == PlayerController.User)
                        {
                            yield return builder.GenerateSetPlayerAllianceStatement(
                                nameof(War3Api.Common.SetPlayerAlliance),
                                nameof(War3Api.Common.Player),
                                nameof(War3Api.Common.ALLIANCE_RESCUABLE),
                                playerData.PlayerNumber,
                                otherPlayerData.PlayerNumber,
                                true);
                        }
                    }
                }
            }

            if (!mapInfo.MapFlags.HasFlag(MapFlags.UseCustomForces))
            {
                for (var i = 0; i < playerDataCount; i++)
                {
                    var playerData = mapInfo.GetPlayerData(i);

                    yield return builder.GenerateSetPlayerPropertyToVariableStatement(
                        nameof(War3Api.Blizzard.SetPlayerSlotAvailable),
                        nameof(War3Api.Common.Player),
                        playerData.PlayerNumber,
                        nameof(War3Api.Common.MAP_CONTROL_USER));
                }

                yield return builder.GenerateInvocationStatementWithoutArguments(
                    nameof(War3Api.Blizzard.InitGenericPlayerSlots));
            }

            // InitCustomTeams
            for (var i = 0; i < forceDataCount; i++)
            {
                var forceData = mapInfo.GetForceData(i);

                var playerSlots = new List<int>();
                foreach (var playerSlot in forceData.GetPlayers())
                {
                    if (mapInfo.PlayerExists(playerSlot))
                    {
                        playerSlots.Add(playerSlot);
                    }
                }

                var alliedVictory = forceData.ForceFlags.HasFlag(ForceFlags.AlliedVictory);
                foreach (var playerSlot in playerSlots)
                {
                    yield return builder.GenerateSetPlayerTeamStatement(
                        nameof(War3Api.Common.SetPlayerTeam),
                        nameof(War3Api.Common.Player),
                        playerSlot,
                        i);

                    if (alliedVictory)
                    {
                        yield return builder.GenerateSetPlayerStateStatement(
                            nameof(War3Api.Common.SetPlayerState),
                            nameof(War3Api.Common.Player),
                            nameof(War3Api.Common.PLAYER_STATE_ALLIED_VICTORY),
                            playerSlot,
                            1);
                    }
                }

                IEnumerable<(int playerSlot1, int playerSlot2)> GetPlayerPairs()
                {
                    foreach (var playerSlot1 in playerSlots)
                    {
                        foreach (var playerSlot2 in playerSlots)
                        {
                            if (playerSlot1 != playerSlot2)
                            {
                                yield return (playerSlot1, playerSlot2);
                            }
                        }
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.Allied))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateSetPlayerAllianceStateStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateAllyBJ),
                            nameof(War3Api.Common.Player),
                            playerSlot1,
                            playerSlot2,
                            true);
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareVision))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateSetPlayerAllianceStateStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateVisionBJ),
                            nameof(War3Api.Common.Player),
                            playerSlot1,
                            playerSlot2,
                            true);
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareUnitControl))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateSetPlayerAllianceStateStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateControlBJ),
                            nameof(War3Api.Common.Player),
                            playerSlot1,
                            playerSlot2,
                            true);
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateSetPlayerAllianceStateStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateFullControlBJ),
                            nameof(War3Api.Common.Player),
                            playerSlot1,
                            playerSlot2,
                            true);
                    }
                }
            }

            // InitAllyPriorities
            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.GetPlayerData(i);

                var slotIndex = 0;
                var substatements = new List<TStatementSyntax>();
                foreach (var (index, highPriority) in playerData.GetStartLocationPriorities())
                {
                    substatements.Add(builder.GenerateSetStartLocPrioStatement(
                        nameof(War3Api.Common.SetStartLocPrio),
                        i,
                        slotIndex++,
                        index,
                        highPriority ? nameof(War3Api.Common.MAP_LOC_PRIO_HIGH) : nameof(War3Api.Common.MAP_LOC_PRIO_LOW)));
                }

                yield return builder.GenerateSetStartLocPrioCountStatement(
                    nameof(War3Api.Common.SetStartLocPrioCount),
                    i,
                    slotIndex);

                foreach (var substatement in substatements)
                {
                    yield return substatement;
                }
            }
        }
    }
}