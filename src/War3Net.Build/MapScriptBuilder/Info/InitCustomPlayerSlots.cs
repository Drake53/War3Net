// ------------------------------------------------------------------------------
// <copyright file="InitCustomPlayerSlots.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitCustomPlayerSlots(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{nameof(InitCustomPlayerSlots)}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            var playerDataCount = mapInfo.Players.Count;

            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.Players[i];

                //statements.Add(JassEmptySyntax.Value);
                //statements.Add(new JassCommentSyntax($" Player {playerData.Id}"));

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerStartLocation,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i))));

                if (playerData.Flags.HasFlag(PlayerFlags.FixedStartPosition))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.ForcePlayerStartLocation,
                        SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i))));
                }

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerColor,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                    SyntaxFactory.InvocationExpression(NativeName.ConvertPlayerColor, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id)))));

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerRacePreference,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                    SyntaxFactory.ParseIdentifierName(playerData.Race.GetVariableName())));

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerRaceSelectable,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Flags.HasFlag(PlayerFlags.RaceSelectable) || !mapInfo.MapFlags.HasFlag(MapFlags.FixedPlayerSettingsForCustomForces)))));

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetPlayerController,
                    SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                    SyntaxFactory.ParseIdentifierName(playerData.Controller.GetVariableName())));

                if (playerData.Controller == PlayerController.Rescuable)
                {
                    for (var j = 0; j < playerDataCount; j++)
                    {
                        var otherPlayerData = mapInfo.Players[j];
                        if (otherPlayerData.Controller == PlayerController.User)
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                NativeName.SetPlayerAlliance,
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(playerData.Id))),
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(otherPlayerData.Id))),
                                SyntaxFactory.ParseIdentifierName(AllianceTypeName.Rescuable),
                                SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                        }
                    }
                }
            }

            //statements.Add(JassEmptySyntax.Value);

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitCustomPlayerSlots)), statements);
        }

        protected internal virtual bool InitCustomPlayerSlotsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }
    }
}