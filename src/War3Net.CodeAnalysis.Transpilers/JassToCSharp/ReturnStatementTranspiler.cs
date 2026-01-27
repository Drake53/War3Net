// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers.Extensions;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassReturnStatementSyntax returnStatement)
        {
            var returnKeyword = Transpile(SyntaxKind.ReturnKeyword, returnStatement.ReturnToken);

            ExpressionSyntax? expression;
            SyntaxTriviaList trailingTrivia;

            if (returnStatement.Expression is null)
            {
                expression = null;
                trailingTrivia = returnKeyword.TrailingTrivia;
                returnKeyword = returnKeyword.WithoutTrailingTrivia();
            }
            else
            {
                expression = Transpile(returnStatement.Expression);
                trailingTrivia = expression.GetTrailingTrivia();
                expression = expression.WithoutTrailingTrivia();
            }

            return SyntaxFactory.ReturnStatement(
                returnKeyword,
                expression,
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}