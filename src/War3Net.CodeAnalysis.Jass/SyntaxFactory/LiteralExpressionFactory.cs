// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassExpressionSyntax LiteralExpression(string? value)
        {
            return LiteralExpression(value is null ? Token(JassSyntaxKind.NullKeyword) : Token(JassSyntaxKind.StringLiteralToken, $"\"{value}\""));
        }

        public static JassExpressionSyntax LiteralExpression(int value)
        {
            if (value == 0)
            {
                return LiteralExpression(Token(JassSyntaxKind.OctalLiteralToken, JassSymbol.Zero));
            }

            var valueAsString = value.ToString(CultureInfo.InvariantCulture);
            if (valueAsString.StartsWith(JassSymbol.MinusChar))
            {
                return UnaryMinusExpression(LiteralExpression(Token(JassSyntaxKind.DecimalLiteralToken, valueAsString.TrimStart(JassSymbol.MinusChar))));
            }

            return LiteralExpression(Token(JassSyntaxKind.DecimalLiteralToken, valueAsString));
        }

        public static JassExpressionSyntax LiteralExpression(float value, int precision = 1)
        {
            var valueAsString = value.ToString($"F{precision}", CultureInfo.InvariantCulture);
            if (precision == 0)
            {
                valueAsString += JassSymbol.Dot;
            }

            if (valueAsString.StartsWith(JassSymbol.MinusChar))
            {
                return UnaryMinusExpression(LiteralExpression(Token(JassSyntaxKind.RealLiteralToken, valueAsString.TrimStart(JassSymbol.MinusChar))));
            }

            return LiteralExpression(Token(JassSyntaxKind.RealLiteralToken, valueAsString));
        }

        public static JassExpressionSyntax LiteralExpression(bool value)
        {
            return LiteralExpression(Token(value ? JassSyntaxKind.TrueKeyword : JassSyntaxKind.FalseKeyword));
        }

        public static JassExpressionSyntax FourCCLiteralExpression(int value)
        {
            return LiteralExpression(Token(JassSyntaxKind.FourCCLiteralToken, $"'{value.ToJassRawcode()}'"));
        }

        private static JassExpressionSyntax LiteralExpression(JassSyntaxToken token)
        {
            return new JassLiteralExpressionSyntax(token);
        }
    }
}