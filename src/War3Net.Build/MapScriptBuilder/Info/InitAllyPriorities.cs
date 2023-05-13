﻿// ------------------------------------------------------------------------------
// <copyright file="InitAllyPriorities.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitAllyPriorities(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{nameof(InitAllyPriorities)}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            var playerDataCount = mapInfo.Players.Count;
            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.Players[i];

                var allyStartLocPrioStatements = new List<JassStatementSyntax>();
                var enemyStartLocPrioStatements = new List<JassStatementSyntax>();

                var allySlotIndex = 0;
                var enemySlotIndex = 0;
                for (var j = 0; j < MaxPlayerSlots; j++)
                {
                    var hasLowFlag = playerData.AllyLowPriorityFlags[j];
                    var hasHighFlag = playerData.AllyHighPriorityFlags[j];
                    if (hasLowFlag || hasHighFlag)
                    {
                        allyStartLocPrioStatements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetStartLocPrio,
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(allySlotIndex++)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(j)),
                            SyntaxFactory.ParseIdentifierName(hasHighFlag ? StartLocPrioName.High : StartLocPrioName.Low)));
                    }

                    if (mapInfo.FormatVersion >= MapInfoFormatVersion.v31)
                    {
                        hasLowFlag = playerData.EnemyLowPriorityFlags[j];
                        hasHighFlag = playerData.EnemyHighPriorityFlags[j];
                        if (hasLowFlag || hasHighFlag)
                        {
                            enemyStartLocPrioStatements.Add(SyntaxFactory.CallStatement(
                                NativeName.SetEnemyStartLocPrio,
                                SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i)),
                                SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(enemySlotIndex++)),
                                SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(j)),
                                SyntaxFactory.ParseIdentifierName(hasHighFlag ? StartLocPrioName.High : StartLocPrioName.Low)));
                        }
                    }
                }

                //statements.Add(JassEmptySyntax.Value);

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetStartLocPrioCount,
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i)),
                    SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(allySlotIndex))));

                statements.AddRange(allyStartLocPrioStatements);

                if (enemyStartLocPrioStatements.Count > 0)
                {
                    //statements.Add(JassEmptySyntax.Value);

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetEnemyStartLocPrioCount,
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(enemySlotIndex))));

                    statements.AddRange(enemyStartLocPrioStatements);
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitAllyPriorities)), statements);
        }

        protected internal virtual bool InitAllyPrioritiesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is null)
            {
                return false;
            }

            var ids = Enumerable.Range(0, MaxPlayerSlots).ToArray();
            return map.Info.Players.Any(p => ids.Any(id => p.AllyLowPriorityFlags[id] || p.AllyHighPriorityFlags[id]));
        }
    }
}