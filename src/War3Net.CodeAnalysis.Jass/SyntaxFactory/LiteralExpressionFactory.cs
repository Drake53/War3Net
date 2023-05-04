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
            return value is null ? Token(JassSyntaxKind.NullKeyword) : Token(JassSyntaxKind.StringLiteralToken, $"\"{value}\"");
        }

        public static JassSyntaxToken Literal(int value)
        {
            return Token(JassSyntaxKind.DecimalLiteralToken, value.ToString(CultureInfo.InvariantCulture));
        }

        public static JassSyntaxToken Literal(float value, int precision = 1)
        {
            var valueAsString = value.ToString($"F{precision}", CultureInfo.InvariantCulture);
            if (precision == 0)
            {
                valueAsString += JassSymbol.Dot;
            }

            return Token(JassSyntaxKind.RealLiteralToken, valueAsString);
        }

        public static JassSyntaxToken Literal(bool value)
        {
            return Token(value ? JassSyntaxKind.TrueKeyword : JassSyntaxKind.FalseKeyword);
        }

        public static JassSyntaxToken FourCCLiteral(int value)
        {
            return Token(JassSyntaxKind.FourCCLiteralToken, $"'{value.ToJassRawcode()}'");
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