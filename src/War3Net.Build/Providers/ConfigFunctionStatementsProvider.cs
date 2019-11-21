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
    internal static class ConfigFunctionProvider
    {
        public const string FunctionName = "config";
    }

    internal static class ConfigFunctionStatementsProvider<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        public static IEnumerable<TStatementSyntax> GetStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;

            var playerDataCount = mapInfo.PlayerDataCount;
            var forceDataCount = mapInfo.ForceDataCount;

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetMapName),
                builder.GenerateStringLiteralExpression(mapInfo.MapName));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetMapDescription),
                builder.GenerateStringLiteralExpression(mapInfo.MapDescription));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetPlayers),
                builder.GenerateIntegerLiteralExpression(playerDataCount));

            // Intuitively this would use forceDataCount, but so far all examples generated with World Editor have same argument for SetPlayers and SetTeams.
            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetTeams),
                builder.GenerateIntegerLiteralExpression(playerDataCount));

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.SetGamePlacement),
                builder.GenerateVariableExpression(nameof(War3Api.Common.MAP_PLACEMENT_TEAMS_TOGETHER)));

            if (builder.Data.LobbyMusic != null)
            {
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.PlayMusic),
                    builder.GenerateStringLiteralExpression(builder.Data.LobbyMusic));
            }

            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.GetPlayerData(i);

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.DefineStartLocation),
                    builder.GenerateIntegerLiteralExpression(i),
                    builder.GenerateFloatLiteralExpression(playerData.StartPosition.X),
                    builder.GenerateFloatLiteralExpression(playerData.StartPosition.Y));
            }

            // InitCustomPlayerSlots
            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.GetPlayerData(i);

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetPlayerStartLocation),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.Player),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                    builder.GenerateIntegerLiteralExpression(i));

                if (playerData.FixedStartPosition)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.ForcePlayerStartLocation),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                        builder.GenerateIntegerLiteralExpression(i));
                }

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetPlayerColor),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.Player),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.ConvertPlayerColor),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)));

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetPlayerRacePreference),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.Player),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                    builder.GenerateVariableExpression(RacePreferenceProvider.GetRacePreferenceString(playerData.PlayerRace)));

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetPlayerRaceSelectable),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.Player),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                    builder.GenerateBooleanLiteralExpression(playerData.IsRaceSelectable || !mapInfo.MapFlags.HasFlag(MapFlags.FixedPlayerSettingsForCustomForces)));

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetPlayerController),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.Player),
                        builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                    builder.GenerateVariableExpression(PlayerControllerProvider.GetPlayerControllerString(playerData.PlayerController)));

                if (playerData.PlayerController == PlayerController.Rescuable)
                {
                    for (var j = 0; j < playerDataCount; j++)
                    {
                        var otherPlayerData = mapInfo.GetPlayerData(j);
                        if (otherPlayerData.PlayerController == PlayerController.User)
                        {
                            yield return builder.GenerateInvocationStatement(
                                nameof(War3Api.Common.SetPlayerAlliance),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.Player),
                                    builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.Player),
                                    builder.GenerateIntegerLiteralExpression(otherPlayerData.PlayerNumber)),
                                builder.GenerateVariableExpression(nameof(War3Api.Common.ALLIANCE_RESCUABLE)),
                                builder.GenerateBooleanLiteralExpression(true));
                        }
                    }
                }
            }

            if (!mapInfo.MapFlags.HasFlag(MapFlags.UseCustomForces))
            {
                for (var i = 0; i < playerDataCount; i++)
                {
                    var playerData = mapInfo.GetPlayerData(i);

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Blizzard.SetPlayerSlotAvailable),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(playerData.PlayerNumber)),
                        builder.GenerateVariableExpression(nameof(War3Api.Common.MAP_CONTROL_USER)));
                }

                yield return builder.GenerateInvocationStatement(
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
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetPlayerTeam),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Player),
                            builder.GenerateIntegerLiteralExpression(playerSlot)),
                        builder.GenerateIntegerLiteralExpression(i));

                    if (alliedVictory)
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetPlayerState),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot)),
                            builder.GenerateVariableExpression(nameof(War3Api.Common.PLAYER_STATE_ALLIED_VICTORY)),
                            builder.GenerateIntegerLiteralExpression(1));
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
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateAllyBJ),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot1)),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot2)),
                            builder.GenerateBooleanLiteralExpression(true));
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareVision))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateVisionBJ),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot1)),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot2)),
                            builder.GenerateBooleanLiteralExpression(true));
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareUnitControl))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateControlBJ),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot1)),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot2)),
                            builder.GenerateBooleanLiteralExpression(true));
                    }
                }

                if (forceData.ForceFlags.HasFlag(ForceFlags.ShareAdvancedUnitControl))
                {
                    foreach (var (playerSlot1, playerSlot2) in GetPlayerPairs())
                    {
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.SetPlayerAllianceStateFullControlBJ),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot1)),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateIntegerLiteralExpression(playerSlot2)),
                            builder.GenerateBooleanLiteralExpression(true));
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
                    substatements.Add(builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetStartLocPrio),
                        builder.GenerateIntegerLiteralExpression(i),
                        builder.GenerateIntegerLiteralExpression(slotIndex),
                        builder.GenerateIntegerLiteralExpression(index),
                        builder.GenerateVariableExpression(highPriority
                            ? nameof(War3Api.Common.MAP_LOC_PRIO_HIGH)
                            : nameof(War3Api.Common.MAP_LOC_PRIO_LOW))));

                    slotIndex++;
                }

                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.SetStartLocPrioCount),
                    builder.GenerateIntegerLiteralExpression(i),
                    builder.GenerateIntegerLiteralExpression(slotIndex));

                foreach (var substatement in substatements)
                {
                    yield return substatement;
                }
            }
        }
    }
}