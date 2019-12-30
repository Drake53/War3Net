// ------------------------------------------------------------------------------
// <copyright file="InitCustomPlayerSlotsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Info;
using War3Net.Build.Providers;

namespace War3Net.Build.Script.Config
{
    internal static partial class ConfigFunctionGenerator<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateInitCustomPlayerSlotsHelperFunction(TBuilder builder)
        {
            return builder.Build("InitCustomPlayerSlots", GetInitCustomPlayerSlotsHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetInitCustomPlayerSlotsHelperFunctionStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            var playerDataCount = mapInfo.PlayerDataCount;

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
        }
    }
}