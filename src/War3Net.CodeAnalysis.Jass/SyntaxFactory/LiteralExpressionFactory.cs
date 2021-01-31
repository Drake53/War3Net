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
            return new JassDecimalLiteralExpressionSyntax(value);
        }

        public static IExpressionSyntax LiteralExpression(float value, int precision = 1)
        {
            var valueAsString = value.ToString($"F{precision}", CultureInfo.InvariantCulture).Split('.');
            return new JassRealLiteralExpressionSyntax(valueAsString[0], valueAsString[1]);
        }

        public static IExpressionSyntax LiteralExpression(bool value)
        {
            return value ? JassBooleanLiteralExpressionSyntax.True : JassBooleanLiteralExpressionSyntax.False;
        }
    }
}