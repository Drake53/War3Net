// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassSetStatementSyntax setStatement)
        {
            var leadingTrivia = MergeTrivia(setStatement.SetToken, setStatement.IdentifierName.GetLeadingTrivia());

            ExpressionSyntax left = setStatement.ElementAccessClause is null
                ? Transpile(leadingTrivia, setStatement.IdentifierName)
                : SyntaxFactory.ElementAccessExpression(
                    Transpile(leadingTrivia, setStatement.IdentifierName),
                    Transpile(setStatement.ElementAccessClause));

            var assignmentExpression = SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                left,
                Transpile(SyntaxKind.EqualsToken, setStatement.EqualsValueClause.EqualsToken),
                Transpile(setStatement.EqualsValueClause.Expression));

            var trailingTrivia = assignmentExpression.GetTrailingTrivia();

            return SyntaxFactory.ExpressionStatement(
                assignmentExpression.WithoutTrailingTrivia(),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Empty,
                    SyntaxKind.SemicolonToken,
                    trailingTrivia));
        }
    }
}