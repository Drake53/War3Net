// ------------------------------------------------------------------------------
// <copyright file="ExpressionSyntaxExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class ExpressionSyntaxExtensions
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
                    value = (int)decimalLiteralExpression.Value;
                    return true;

                case JassOctalLiteralExpressionSyntax octalLiteralExpression:
                    value = octalLiteralExpression.Value;
                    return true;

                case JassFourCCLiteralExpressionSyntax fourCCLiteralExpression:
                    value = fourCCLiteralExpression.Value;
                    return true;

                case JassUnaryExpressionSyntax unaryExpression:
                    return int.TryParse(unaryExpression.ToString(), out value);

                case JassHexadecimalLiteralExpressionSyntax hexLiteralExpression:
                    value = hexLiteralExpression.Value;
                    return true;

                default:
                    value = default;
                    return false;
            }
        }

        public static bool TryGetPlayerIdExpressionValue(this IExpressionSyntax expression, int maxPlayerSlots, out int value)
        {
            if (expression is JassVariableReferenceExpressionSyntax variableReferenceExpression)
            {
                if (string.Equals(variableReferenceExpression.IdentifierName.Name, "PLAYER_NEUTRAL_AGGRESSIVE", StringComparison.Ordinal))
                {
                    value = maxPlayerSlots;
                    return true;
                }
                else if (string.Equals(variableReferenceExpression.IdentifierName.Name, "PLAYER_NEUTRAL_PASSIVE", StringComparison.Ordinal))
                {
                    value = maxPlayerSlots + 3;
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
            else
            {
                return expression.TryGetIntegerExpressionValue(out value);
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