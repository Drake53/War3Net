// ------------------------------------------------------------------------------
// <copyright file="InitTechTree.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax InitTechTree(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            foreach (var player in mapInfo.Players)
            {
                var playerNumber = player.Id;
                foreach (var techData in mapInfo.TechData)
                {
                    if (techData.Players[playerNumber])
                    {
                        if (techData.Id.ToRawcode()[0] == 'A')
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                NativeName.SetPlayerAbilityAvailable,
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerNumber)),
                                SyntaxFactory.FourCCLiteralExpression(techData.Id),
                                SyntaxFactory.LiteralExpression(false)));
                        }
                        else
                        {
                            statements.Add(SyntaxFactory.CallStatement(
                                NativeName.SetPlayerTechMaxAllowed,
                                SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.LiteralExpression(playerNumber)),
                                SyntaxFactory.FourCCLiteralExpression(techData.Id),
                                SyntaxFactory.LiteralExpression(0)));
                        }
                    }
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitTechTree)), statements);
        }

        protected virtual bool InitTechTreeCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.TechData.Any();
        }
    }
}