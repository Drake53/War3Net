// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassCallStatementSyntax callStatement)
        {
            var leadingTrivia = MergeTrivia(
                callStatement.CallToken,
                callStatement.IdentifierName.Token.LeadingTrivia);

            var invocationExpression = SyntaxFactory.InvocationExpression(
                Transpile(leadingTrivia, callStatement.IdentifierName),
                Transpile(callStatement.ArgumentList));

            var trailingTrivia = invocationExpression.GetTrailingTrivia();

            return SyntaxFactory.ExpressionStatement(
                invocationExpression.WithoutTrailingTrivia(),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}