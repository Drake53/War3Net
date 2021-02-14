// ------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable IDE1006, SA1300

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
        protected virtual JassFunctionDeclarationSyntax config(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            var playerDataCount = mapInfo.Players.Count;
            var forceDataCount = mapInfo.Forces.Count;

            statements.Add(SyntaxFactory.CallStatement(nameof(SetMapName), SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(mapInfo.MapName))));
            statements.Add(SyntaxFactory.CallStatement(nameof(SetMapDescription), SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(mapInfo.MapDescription))));
            statements.Add(SyntaxFactory.CallStatement(nameof(SetPlayers), SyntaxFactory.LiteralExpression(playerDataCount)));
            statements.Add(SyntaxFactory.CallStatement(nameof(SetTeams), SyntaxFactory.LiteralExpression(playerDataCount)));
            statements.Add(SyntaxFactory.CallStatement(nameof(SetGamePlacement), SyntaxFactory.VariableReferenceExpression(nameof(MAP_PLACEMENT_USE_MAP_SETTINGS))));
            statements.Add(JassEmptyStatementSyntax.Value);

            if (!string.IsNullOrEmpty(LobbyMusic))
            {
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(PlayMusic),
                    SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(LobbyMusic))));
            }

            for (var i = 0; i < playerDataCount; i++)
            {
                var location = mapInfo.Players[i].StartPosition;
                statements.Add(SyntaxFactory.CallStatement(
                    nameof(DefineStartLocation),
                    SyntaxFactory.LiteralExpression(i),
                    SyntaxFactory.LiteralExpression(location.X, precision: 1),
                    SyntaxFactory.LiteralExpression(location.Y, precision: 1)));
            }

            statements.Add(JassEmptyStatementSyntax.Value);
            statements.Add(new JassCommentStatementSyntax(" Player setup"));

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
                        nameof(War3Api.Blizzard.SetPlayerSlotAvailable),
                        SyntaxFactory.InvocationExpression(nameof(Player), SyntaxFactory.LiteralExpression(mapInfo.Players[i].Id)),
                        SyntaxFactory.VariableReferenceExpression(nameof(MAP_CONTROL_USER))));
                }

                elseStatements.Add(SyntaxFactory.CallStatement(nameof(War3Api.Blizzard.InitGenericPlayerSlots)));
            }

            statements.AddRange(elseStatements);

            if (InitCustomTeamsCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitCustomTeams)));
            }

            if (InitAllyPrioritiesCondition(map))
            {
                statements.Add(SyntaxFactory.CallStatement(nameof(InitAllyPriorities)));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(config)), statements);
        }

        protected virtual bool configCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}