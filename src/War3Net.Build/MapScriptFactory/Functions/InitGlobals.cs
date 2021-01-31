// ------------------------------------------------------------------------------
// <copyright file="InitGlobals.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public static partial class MapScriptFactory
    {
        public static JassFunctionDeclarationSyntax InitGlobals(MapTriggers mapTriggers)
        {
            const string LocalIndexVariableName = "i";

            var statements = new List<IStatementSyntax>();

            if (mapTriggers.Variables.Any(variable => variable.IsArray))
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Integer, LocalIndexVariableName, SyntaxFactory.LiteralExpression(0)));
            }

            if (mapTriggers is not null)
            {
                foreach (var variable in mapTriggers.Variables)
                {
                    if (variable.IsArray)
                    {
                        statements.Add(SyntaxFactory.SetStatement(LocalIndexVariableName, SyntaxFactory.LiteralExpression(0)));
                        statements.Add(SyntaxFactory.LoopStatement(
                            new JassExitStatementSyntax(new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryGreaterThanExpression(
                                SyntaxFactory.VariableReferenceExpression(LocalIndexVariableName),
                                SyntaxFactory.LiteralExpression(variable.ArraySize)))),
                            SyntaxFactory.SetStatement(variable.GetVariableName(), SyntaxFactory.VariableReferenceExpression(LocalIndexVariableName), variable.GetInitialValueExpression()),
                            SyntaxFactory.SetStatement(LocalIndexVariableName, SyntaxFactory.BinaryAdditionExpression(SyntaxFactory.VariableReferenceExpression(LocalIndexVariableName), SyntaxFactory.LiteralExpression(1)))));
                    }
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitGlobals)), statements);
        }
    }
}