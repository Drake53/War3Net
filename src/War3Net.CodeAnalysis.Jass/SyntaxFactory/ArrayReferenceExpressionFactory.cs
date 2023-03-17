// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassArrayReferenceExpressionSyntax ArrayReferenceExpression(JassIdentifierNameSyntax identifierName, JassElementAccessClauseSyntax elementAccessClause)
        {
            return new JassArrayReferenceExpressionSyntax(
                identifierName,
                elementAccessClause);
        }

        public static JassArrayReferenceExpressionSyntax ArrayReferenceExpression(JassIdentifierNameSyntax identifierName, JassExpressionSyntax elementAccessExpression)
        {
            return new JassArrayReferenceExpressionSyntax(
                identifierName,
                ElementAccessClause(elementAccessExpression));
        }

        public static JassArrayReferenceExpressionSyntax ArrayReferenceExpression(string name, JassElementAccessClauseSyntax elementAccessClause)
        {
            return new JassArrayReferenceExpressionSyntax(
                ParseIdentifierName(name),
                elementAccessClause);
        }

        public static JassArrayReferenceExpressionSyntax ArrayReferenceExpression(string name, JassExpressionSyntax elementAccessExpression)
        {
            return new JassArrayReferenceExpressionSyntax(
                ParseIdentifierName(name),
                ElementAccessClause(elementAccessExpression));
        }
    }
}