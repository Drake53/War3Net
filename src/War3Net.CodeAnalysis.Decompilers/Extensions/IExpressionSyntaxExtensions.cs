// ------------------------------------------------------------------------------
// <copyright file="IExpressionSyntaxExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers.Extensions
{
    public static class IExpressionSyntaxExtensions
    {
        public static IExpressionSyntax Deparenthesize(this IExpressionSyntax expression)
        {
            while (expression is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                expression = parenthesizedExpression.Expression;
            }

            return expression;
        }

        public static bool TryGetIntegerExpressionValue(this IExpressionSyntax expression, out int value)
        {
            switch (expression)
            {
                case JassDecimalLiteralExpressionSyntax decimalLiteralExpression:
                    value = decimalLiteralExpression.Value;
                    return true;

                case JassOctalLiteralExpressionSyntax octalLiteralExpression:
                    value = octalLiteralExpression.Value;
                    return true;

                default:
                    value = default;
                    return false;
            }
        }

        public static bool TryGetRealExpressionValue(this IExpressionSyntax expression, out float value)
        {
            if (expression.TryGetIntegerExpressionValue(out var intValue))
            {
                value = intValue;
                return true;
            }

            if (expression is JassRealLiteralExpressionSyntax realLiteralExpression &&
                float.TryParse(realLiteralExpression.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            if (expression is JassUnaryExpressionSyntax unaryExpression &&
                float.TryParse(unaryExpression.ToString(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            value = default;
            return false;
        }
    }
}