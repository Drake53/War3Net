// ------------------------------------------------------------------------------
// <copyright file="InitCustomPlayerSlots.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax InitCustomPlayerSlots(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            var playerDataCount = mapInfo.Players.Count;

            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.Players[i];

                statements.Add(JassEmptyStatementSyntax.Value);
                statements.Add(new JassCommentStatementSyntax($" Player {playerData.Id}"));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetPlayerStartLocation),
                    SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                    SyntaxFactory.LiteralExpression(i)));

                if (playerData.Flags.HasFlag(PlayerFlags.FixedStartPosition))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(ForcePlayerStartLocation),
                        SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                        SyntaxFactory.LiteralExpression(i)));
                }

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetPlayerColor),
                    SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                    SyntaxFactory.InvocationExpression(nameof(ConvertPlayerColor), SyntaxFactory.LiteralExpression(playerData.Id))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetPlayerRacePreference),
                    SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                    SyntaxFactory.VariableReferenceExpression(RacePreferenceProvider.GetRacePreferenceString(playerData.Race))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetPlayerRaceSelectable),
                    SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                    SyntaxFactory.LiteralExpression(playerData.Flags.HasFlag(PlayerFlags.RaceSelectable) || !mapInfo.MapFlags.HasFlag(MapFlags.FixedPlayerSettingsForCustomForces))));

                statements.Add(SyntaxFactory.CallStatement(
                    nameof(SetPlayerController),
                    SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                    SyntaxFactory.VariableReferenceExpression(PlayerControllerProvider.GetPlayerControllerString(playerData.Controller))));

                if (playerData.Controller == PlayerController.Rescuable)
                {
                    for (var j = 0; j < playerDataCount; j++)
                    {
                        var otherPlayerData = mapInfo.Players[j];
                        if (otherPlayerData.Controller == PlayerController.User)
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                nameof(SetPlayerAlliance),
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(playerData.Id)),
                                SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(otherPlayerData.Id)),
                                SyntaxFactory.VariableReferenceExpression(nameof(ALLIANCE_RESCUABLE)),
                                SyntaxFactory.LiteralExpression(true)));
                        }
                    }
                }
            }

            statements.Add(JassEmptyStatementSyntax.Value);

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitCustomPlayerSlots)), statements);
        }

        protected virtual bool InitCustomPlayerSlotsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}