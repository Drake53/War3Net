// ------------------------------------------------------------------------------
// <copyright file="SetStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax SetStatement(string variableName, EqualsValueClauseSyntax equalsValueClause)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new SetStatementSyntax(
                        Token(SyntaxTokenType.SetKeyword),
                        Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                        Empty(),
                        equalsValueClause)),
                Newlines());
        }

        public static NewStatementSyntax SetStatement(string variableName, NewExpressionSyntax arrayIndex, EqualsValueClauseSyntax equalsValueClause)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new SetStatementSyntax(
                        Token(SyntaxTokenType.SetKeyword),
                        Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                        BracketedExpression(arrayIndex),
                        equalsValueClause)),
                Newlines());
        }

        public static NewStatementSyntax SetStatement(string variableName, NewExpressionSyntax equalsValueExpression)
        {
            return SetStatement(variableName, EqualsValueClause(equalsValueExpression));
        }

        public static NewStatementSyntax SetStatement(string variableName, NewExpressionSyntax arrayIndex, NewExpressionSyntax equalsValueExpression)
        {
            return SetStatement(variableName, arrayIndex, EqualsValueClause(equalsValueExpression));
        }
    }
}