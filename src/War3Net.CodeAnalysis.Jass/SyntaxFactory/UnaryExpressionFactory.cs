// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassUnaryExpressionSyntax UnaryPlusExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.PlusToken),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryMinusExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.MinusToken),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryNotExpression(JassExpressionSyntax expression)
        {
            return new JassUnaryExpressionSyntax(
                Token(JassSyntaxKind.NotKeyword),
                expression);
        }

        public static JassUnaryExpressionSyntax UnaryNotExpression(JassSyntaxToken operatorToken, JassExpressionSyntax expression)
        {
            var expressionKind = operatorToken.SyntaxKind switch
            {
                JassSyntaxKind.PlusToken => JassSyntaxKind.UnaryPlusExpression,
                JassSyntaxKind.MinusToken => JassSyntaxKind.UnaryMinusExpression,
                JassSyntaxKind.NotKeyword => JassSyntaxKind.NotExpression,

                _ => throw new ArgumentException("Invalid SyntaxKind.", nameof(operatorToken)),
            };

            return new JassUnaryExpressionSyntax(
                operatorToken,
                expression);
        }
    }
}