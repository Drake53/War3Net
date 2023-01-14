// ------------------------------------------------------------------------------
// <copyright file="JassExpressionSyntaxExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class JassExpressionSyntaxExtensions
    {
        public static JassExpressionSyntax Deparenthesize(this JassExpressionSyntax expression)
        {
            while (expression is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                expression = parenthesizedExpression.Expression;
            }

            return expression;
        }

        public static bool TryGetIntegerExpressionValue(this JassExpressionSyntax expression, out int value)
        {
            if (expression is JassLiteralExpressionSyntax literalExpression)
            {
                switch (literalExpression.Token.SyntaxKind)
                {
                    case JassSyntaxKind.DecimalLiteralToken:
                        value = int.Parse(literalExpression.Token.Text, CultureInfo.InvariantCulture);
                        return true;

                    case JassSyntaxKind.OctalLiteralToken:
                        value = Convert.ToInt32(literalExpression.Token.Text, 8);
                        return true;

                    case JassSyntaxKind.FourCCLiteralToken:
                        value = literalExpression.Token.Text[1..^1].FromJassRawcode();
                        return true;

                    default:
                        value = default;
                        return false;
                }
            }
            else if (expression is JassUnaryExpressionSyntax unaryExpression)
            {
                switch (unaryExpression.OperatorToken.SyntaxKind)
                {
                    case JassSyntaxKind.PlusToken:
                        return unaryExpression.Expression.TryGetIntegerExpressionValue(out value);

                    case JassSyntaxKind.MinusToken:
                        if (unaryExpression.Expression.TryGetIntegerExpressionValue(out var result))
                        {
                            value = -result;
                            return true;
                        }

                        break;

                    default:
                        value = default;
                        return false;
                }
            }

            value = default;
            return false;
        }

        public static bool TryGetPlayerIdExpressionValue(this JassExpressionSyntax expression, int maxPlayerSlots, out int value)
        {
            if (expression is JassIdentifierNameSyntax identifierName)
            {
                if (string.Equals(identifierName.Token.Text, "PLAYER_NEUTRAL_AGGRESSIVE", StringComparison.Ordinal))
                {
                    value = maxPlayerSlots;
                    return true;
                }
                else if (string.Equals(identifierName.Token.Text, "PLAYER_NEUTRAL_PASSIVE", StringComparison.Ordinal))
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

        public static bool TryGetRealExpressionValue(this JassExpressionSyntax expression, out float value)
        {
            if (expression.TryGetIntegerExpressionValue(out var intValue))
            {
                value = intValue;
                return true;
            }

            if (expression is JassLiteralExpressionSyntax literalExpression &&
                float.TryParse(literalExpression.Token.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
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