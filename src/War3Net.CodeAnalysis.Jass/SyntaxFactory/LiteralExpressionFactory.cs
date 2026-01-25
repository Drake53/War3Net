// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassSyntaxToken Literal(string? value)
        {
            return Literal(JassSyntaxTriviaList.Empty, value, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Literal(JassSyntaxTriviaList leadingTrivia, string? value, JassSyntaxTriviaList trailingTrivia)
        {
            return value is null
                ? Token(leadingTrivia, JassSyntaxKind.NullKeyword, trailingTrivia)
                : Token(leadingTrivia, JassSyntaxKind.StringLiteralToken, $"\"{value}\"", trailingTrivia);
        }

        public static JassSyntaxToken Literal(int value)
        {
            return Literal(JassSyntaxTriviaList.Empty, value, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Literal(JassSyntaxTriviaList leadingTrivia, int value, JassSyntaxTriviaList trailingTrivia)
        {
            return Token(leadingTrivia, JassSyntaxKind.DecimalLiteralToken, value.ToString(CultureInfo.InvariantCulture), trailingTrivia);
        }

        public static JassSyntaxToken Literal(float value, int precision = 1)
        {
            return Literal(JassSyntaxTriviaList.Empty, value, precision, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Literal(JassSyntaxTriviaList leadingTrivia, float value, int precision, JassSyntaxTriviaList trailingTrivia)
        {
            var valueAsString = value.ToString($"F{precision}", CultureInfo.InvariantCulture);
            if (precision == 0)
            {
                valueAsString += JassSymbol.Dot;
            }

            return Token(leadingTrivia, JassSyntaxKind.RealLiteralToken, valueAsString, trailingTrivia);
        }

        public static JassSyntaxToken Literal(bool value)
        {
            return Literal(JassSyntaxTriviaList.Empty, value, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken Literal(JassSyntaxTriviaList leadingTrivia, bool value, JassSyntaxTriviaList trailingTrivia)
        {
            return value
                ? Token(leadingTrivia, JassSyntaxKind.TrueKeyword, JassKeyword.True, trailingTrivia)
                : Token(leadingTrivia, JassSyntaxKind.FalseKeyword, JassKeyword.False, trailingTrivia);
        }

        public static JassSyntaxToken FourCCLiteral(int value)
        {
            return FourCCLiteral(JassSyntaxTriviaList.Empty, value, JassSyntaxTriviaList.Empty);
        }

        public static JassSyntaxToken FourCCLiteral(JassSyntaxTriviaList leadingTrivia, int value, JassSyntaxTriviaList trailingTrivia)
        {
            return Token(leadingTrivia, JassSyntaxKind.FourCCLiteralToken, $"'{value.ToJassRawcode()}'", trailingTrivia);
        }

        public static JassExpressionSyntax LiteralExpression(JassSyntaxToken token)
        {
            if (!JassSyntaxFacts.IsLiteralExpressionToken(token.SyntaxKind))
            {
                throw new ArgumentException("Token kind must be a literal.", nameof(token));
            }

            return new JassLiteralExpressionSyntax(token);
        }
    }
}