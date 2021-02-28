// ------------------------------------------------------------------------------
// <copyright file="InitAllyPriorities.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax InitAllyPriorities(Map map)
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
                statements.Add(JassEmptyStatementSyntax.Value);

                var playerData = mapInfo.Players[i];

                var startLocPrioStatements = new List<IStatementSyntax>();

                var slotIndex = 0;
                for (var j = 0; j < MaxPlayerSlots; j++)
                {
                    var hasLowFlag = playerData.AllyLowPriorityFlags[j];
                    var hasHighFlag = playerData.AllyHighPriorityFlags[j];
                    if (hasLowFlag || hasHighFlag)
                    {
                        startLocPrioStatements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetStartLocPrio,
                            SyntaxFactory.LiteralExpression(i),
                            SyntaxFactory.LiteralExpression(slotIndex++),
                            SyntaxFactory.LiteralExpression(j),
                            SyntaxFactory.VariableReferenceExpression(hasHighFlag ? StartLocPrioName.High : StartLocPrioName.Low)));
                    }
                }

                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.SetStartLocPrioCount,
                    SyntaxFactory.LiteralExpression(i),
                    SyntaxFactory.LiteralExpression(slotIndex)));

                statements.AddRange(startLocPrioStatements);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitAllyPriorities)), statements);
        }

        protected virtual bool InitAllyPrioritiesCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var ids = Enumerable.Range(0, MaxPlayerSlots).ToArray();
            return map.Info.Players.Any(p => ids.Any(id => p.AllyLowPriorityFlags[id] || p.AllyHighPriorityFlags[id]));
        }
    }
}