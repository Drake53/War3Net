// ------------------------------------------------------------------------------
// <copyright file="JassArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static class IJassSyntaxTokenExtensions
    {
        public static IEnumerable<IJassSyntaxToken> GetChildren(this IJassSyntaxToken syntaxToken)
        {
            if (syntaxToken == null)
            {
                return Array.Empty<IJassSyntaxToken>();
            }

            var result = syntaxToken switch
            {
                JassArgumentListSyntax token => token.Arguments.Cast<IJassSyntaxToken>(),
                JassArrayDeclaratorSyntax token => new IJassSyntaxToken[] { token.Type, token.IdentifierName },
                JassArrayReferenceExpressionSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.Indexer },
                JassBinaryExpressionSyntax token => new IJassSyntaxToken[] { token.Left, token.Right },
                JassBooleanLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassCallStatementSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.Arguments },
                JassCharacterLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassCommentSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassCompilationUnitSyntax token => token.Declarations.Cast<IJassSyntaxToken>(),
                JassDebugCustomScriptAction token => new IJassSyntaxToken[] { token.Action },
                JassDebugStatementSyntax token => new IJassSyntaxToken[] { token.Statement },
                JassDecimalLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassElseClauseSyntax token => new IJassSyntaxToken[] { token.Body },
                JassElseCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassElseIfClauseSyntax token => new IJassSyntaxToken[] { token.Condition, token.Body },
                JassElseIfCustomScriptAction token => new IJassSyntaxToken[] { token.Condition },
                JassEmptySyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassEndFunctionCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassEndGlobalsCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassEndIfCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassEndLoopCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassEqualsValueClauseSyntax token => new IJassSyntaxToken[] { token.Expression },
                JassExitStatementSyntax token => new IJassSyntaxToken[] { token.Condition },
                JassFourCCLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassFunctionCustomScriptAction token => new IJassSyntaxToken[] { token.FunctionDeclarator },
                JassFunctionDeclarationSyntax token => new IJassSyntaxToken[] { token.FunctionDeclarator, token.Body },
                JassFunctionDeclaratorSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.ParameterList, token.ReturnType },
                JassFunctionReferenceExpressionSyntax token => new IJassSyntaxToken[] { token.IdentifierName },
                JassGlobalDeclarationListSyntax token => token.Globals.Cast<IJassSyntaxToken>(),
                JassGlobalDeclarationSyntax token => new IJassSyntaxToken[] { token.Declarator },
                JassGlobalsCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassHexadecimalLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassIdentifierNameSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassIfCustomScriptAction token => new IJassSyntaxToken[] { token.Condition },
                JassIfStatementSyntax token => new IJassSyntaxToken[] { token.Condition }
                    .Concat(new[] { token.Body })
                    .Concat(token.ElseIfClauses.Cast<IJassSyntaxToken>())
                    .Concat(new[] { token.ElseClause }),
                JassInvocationExpressionSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.Arguments },
                JassLocalVariableDeclarationStatementSyntax token => new IJassSyntaxToken[] { token.Declarator },
                JassLoopCustomScriptAction _ => Array.Empty<IJassSyntaxToken>(),
                JassLoopStatementSyntax token => new IJassSyntaxToken[] { token.Body },
                JassNativeFunctionDeclarationSyntax token => new IJassSyntaxToken[] { token.FunctionDeclarator },
                JassNullLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassOctalLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassParameterListSyntax token => token.Parameters.Cast<IJassSyntaxToken>(),
                JassParameterSyntax token => new IJassSyntaxToken[] { token.Type, token.IdentifierName },
                JassParenthesizedExpressionSyntax token => new IJassSyntaxToken[] { token.Expression },
                JassRealLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassReturnStatementSyntax token => new IJassSyntaxToken[] { token.Value },
                JassSetStatementSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.Indexer, token.Value },
                JassStatementListSyntax token => token.Statements.Cast<IJassSyntaxToken>(),
                JassStringLiteralExpressionSyntax _ => Array.Empty<IJassSyntaxToken>(),
                JassTypeDeclarationSyntax token => new IJassSyntaxToken[] { token.IdentifierName, token.BaseType },
                JassTypeSyntax token => new IJassSyntaxToken[] { token.TypeName },
                JassUnaryExpressionSyntax token => new IJassSyntaxToken[] { token.Expression },
                JassVariableDeclaratorSyntax token => new IJassSyntaxToken[] { token.Type, token.IdentifierName, token.Value },
                JassVariableReferenceExpressionSyntax token => new IJassSyntaxToken[] { token.IdentifierName },
                _ => throw new ArgumentException($"Argument of type '{syntaxToken?.GetType()?.Name ?? string.Empty}' isn't supported by {nameof(IJassSyntaxTokenExtensions)}.{nameof(GetChildren)} extension method.", nameof(syntaxToken))
            };

            return result.Where(x => x != null);
        }

        public static IEnumerable<IJassSyntaxToken> GetChildren_RecursiveDepthFirst(this IJassSyntaxToken syntaxToken)
        {
            var stack = new Stack<IJassSyntaxToken>();
            stack.Push(syntaxToken);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = current.GetChildren();
                if (children != null)
                {
                    foreach (var child in children.Reverse())
                    {
                        stack.Push(child);
                    }
                }
            }
        }
    }
}
