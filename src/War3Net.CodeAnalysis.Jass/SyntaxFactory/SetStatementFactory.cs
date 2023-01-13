// ------------------------------------------------------------------------------
// <copyright file="SetStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassSetStatementSyntax SetStatement(string name, JassExpressionSyntax value)
        {
            return new JassSetStatementSyntax(
                Token(JassSyntaxKind.SetKeyword),
                ParseIdentifierName(name),
                null,
                EqualsValueClause(value));
        }

        public static JassSetStatementSyntax SetStatement(string name, JassEqualsValueClauseSyntax value)
        {
            return new JassSetStatementSyntax(
                Token(JassSyntaxKind.SetKeyword),
                ParseIdentifierName(name),
                null,
                value);
        }

        public static JassSetStatementSyntax SetStatement(string name, JassExpressionSyntax elementAccessExpression, JassExpressionSyntax value)
        {
            return new JassSetStatementSyntax(
                Token(JassSyntaxKind.SetKeyword),
                ParseIdentifierName(name),
                ElementAccessClause(elementAccessExpression),
                EqualsValueClause(value));
        }

        public static JassSetStatementSyntax SetStatement(string name, JassExpressionSyntax elementAccessExpression, JassEqualsValueClauseSyntax value)
        {
            return new JassSetStatementSyntax(
                Token(JassSyntaxKind.SetKeyword),
                ParseIdentifierName(name),
                ElementAccessClause(elementAccessExpression),
                value);
        }
    }
}