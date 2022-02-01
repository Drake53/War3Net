// ------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable IDE1006, SA1300

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax config(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{nameof(config)}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            var playerDataCount = mapInfo.Players.Count;
            var forceDataCount = mapInfo.Forces.Count;

            statements.Add(SyntaxFactory.CallStatement(NativeName.SetMapName, SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(mapInfo.MapName))));
            statements.Add(SyntaxFactory.CallStatement(NativeName.SetMapDescription, SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(mapInfo.MapDescription))));
            statements.Add(SyntaxFactory.CallStatement(NativeName.SetPlayers, SyntaxFactory.LiteralExpression(playerDataCount)));
            statements.Add(SyntaxFactory.CallStatement(NativeName.SetTeams, SyntaxFactory.LiteralExpression(playerDataCount)));

            if (mapInfo.Players.Any(player => player.AllyHighPriorityFlags != 0 || player.AllyLowPriorityFlags != 0))
            {
                statements.Add(SyntaxFactory.CallStatement(NativeName.SetGamePlacement, SyntaxFactory.VariableReferenceExpression(PlacementName.TeamsTogether)));
            }
            else
            {
                statements.Add(SyntaxFactory.CallStatement(NativeName.SetGamePlacement, SyntaxFactory.VariableReferenceExpression(PlacementName.UseMapSettings)));
            }

            statements.Add(JassEmptySyntax.Value);

            if (!string.IsNullOrEmpty(LobbyMusic))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.PlayMusic,
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(LobbyMusic))));
            }

            for (var i = 0; i < playerDataCount; i++)
            {
                var location = mapInfo.Players[i].StartPosition;
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.DefineStartLocation,
                    SyntaxFactory.LiteralExpression(i),
                    SyntaxFactory.LiteralExpression(location.X, precision: 1),
                    SyntaxFactory.LiteralExpression(location.Y, precision: 1)));
            }

            statements.Add(JassEmptySyntax.Value);
            statements.Add(new JassCommentSyntax(" Player setup"));

            if (InitCustomPlayerSlotsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitCustomPlayerSlots)));
            }

            var elseStatements = new List<IStatementSyntax>();
            if (!mapInfo.MapFlags.HasFlag(MapFlags.UseCustomForces))
            {
                for (var i = 0; i < playerDataCount; i++)
                {
                    elseStatements.Add(SyntaxFactory.CallStatement(
                        FunctionName.SetPlayerSlotAvailable,
                        SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(mapInfo.Players[i].Id)),
                        SyntaxFactory.VariableReferenceExpression(MapControlName.User)));
                }

                elseStatements.Add(SyntaxFactory.CallStatement(FunctionName.InitGenericPlayerSlots));
            }

            if (mapInfo.FormatVersion < MapInfoFormatVersion.v15)
            {
                statements.Add(SyntaxFactory.IfStatement(
                    SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryEqualsExpression(
                        SyntaxFactory.InvocationExpression(NativeName.GetGameTypeSelected),
                        SyntaxFactory.VariableReferenceExpression(GameType.UseMapSettings))),
                    SyntaxFactory.StatementList(SyntaxFactory.CallStatement(nameof(InitCustomTeams))),
                    new JassElseClauseSyntax(SyntaxFactory.StatementList(elseStatements))));
            }
            else
            {
                statements.AddRange(elseStatements);
            }

            if (InitCustomTeamsCondition(map) && InitCustomTeamsInvokeCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitCustomTeams)));
            }

            if (InitAllyPrioritiesCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitAllyPriorities)));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(config)), statements);
        }

        protected internal virtual bool configCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null;
        }
    }
}