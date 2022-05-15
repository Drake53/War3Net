// ------------------------------------------------------------------------------
// <copyright file="LiteralExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static IExpressionSyntax LiteralExpression(string? value)
        {
            return value is null ? JassNullLiteralExpressionSyntax.Value : new JassStringLiteralExpressionSyntax(value);
        }

        public static IExpressionSyntax LiteralExpression(int value)
        {
            if (value == 0)
            {
                return new JassOctalLiteralExpressionSyntax(0);
            }

            if (value < 0)
            {
                return UnaryMinusExpression(new JassDecimalLiteralExpressionSyntax(-value));
            }

            return new JassDecimalLiteralExpressionSyntax(value);
        }

        public static IExpressionSyntax LiteralExpression(float value, int precision = 1)
        {
            var valueAsString = value.ToString($"F{precision}", CultureInfo.InvariantCulture).Split('.', 2);
            if (precision == 0 && valueAsString.Length == 1)
            {
                valueAsString = new string[] { valueAsString[0], string.Empty };
            }

            if (valueAsString[0].StartsWith('-'))
            {
                return UnaryMinusExpression(new JassRealLiteralExpressionSyntax(valueAsString[0].TrimStart('-'), valueAsString[1]));
            }

            return new JassRealLiteralExpressionSyntax(valueAsString[0], valueAsString[1]);
        }

        public static IExpressionSyntax LiteralExpression(bool value)
        {
            return value ? JassBooleanLiteralExpressionSyntax.True : JassBooleanLiteralExpressionSyntax.False;
        }
    }
}