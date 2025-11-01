// ------------------------------------------------------------------------------
// <copyright file="MapSoundsDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
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

        public static T GetValueOrDefault<T>(this IExpressionSyntax expression, T defaultValue = default)
        {
            if (expression.TryGetValue<T>(out var value))
            {
                return value;
            }

            return defaultValue;
        }

        public static bool TryGetValue<T>(this IExpressionSyntax expression, out T value)
        {
            value = default;

            if (!expression.TryGetStringExpressionValue(out var stringValue))
            {
                return false;
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)stringValue;
                return true;
            }

            if (decimal.TryParse(stringValue, out var decimalValue))
            {
                value = SafeConvertDecimalTo<T>(decimalValue);
                return true;
            }

            return false;
        }

        private static T SafeConvertDecimalTo<T>(decimal value)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                var nonNullableType = Nullable.GetUnderlyingType(typeof(T));
                var method = typeof(ExpressionSyntaxExtensions).GetMethod(nameof(SafeConvertDecimalTo),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(nonNullableType);
                var result = genericMethod.Invoke(null, new object[] { value });
                return (T)result;
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)(int)Math.Clamp(value, int.MinValue, int.MaxValue);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)(uint)Math.Clamp(value, uint.MinValue, uint.MaxValue);
            }
            else if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)Math.Clamp(value, byte.MinValue, byte.MaxValue);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)(sbyte)Math.Clamp(value, sbyte.MinValue, sbyte.MaxValue);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(object)(short)Math.Clamp(value, short.MinValue, short.MaxValue);
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)(ushort)Math.Clamp(value, ushort.MinValue, ushort.MaxValue);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)(long)Math.Clamp(value, long.MinValue, long.MaxValue);
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)(ulong)Math.Clamp(value, ulong.MinValue, ulong.MaxValue);
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)(value != 0);
            }
            else if (typeof(T) == typeof(decimal))
            {
                return (T)(object)value;
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)(float)value;
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(object)(double)value;
            }

            return default;
        }

        private static bool TryGetStringExpressionValue(this IExpressionSyntax expression, out string value)
        {
            switch (expression)
            {
                case JassBooleanLiteralExpressionSyntax booleanLiteralExpression:
                    value = booleanLiteralExpression.Value ? "1" : "0";
                    return true;

                case JassDecimalLiteralExpressionSyntax decimalLiteralExpression:
                    value = decimalLiteralExpression.Value.ToString();
                    return true;

                case JassRealLiteralExpressionSyntax realLiteralExpression:
                    value = realLiteralExpression.IntPart + "." + realLiteralExpression.FracPart;
                    return true;

                case JassOctalLiteralExpressionSyntax octalLiteralExpression:
                    value = octalLiteralExpression.Value.ToString();
                    return true;

                case JassFourCCLiteralExpressionSyntax fourCCLiteralExpression:
                    value = fourCCLiteralExpression.Value.ToString();
                    return true;

                case JassUnaryExpressionSyntax unaryExpression:
                    if (unaryExpression.Operator != UnaryOperatorType.Not && TryGetStringExpressionValue(unaryExpression.Expression, out var unaryExpressionValue))
                    {
                        value = (unaryExpression.Operator == UnaryOperatorType.Minus ? "-" : "") + unaryExpressionValue;
                        return true;
                    }
                    break;

                case JassHexadecimalLiteralExpressionSyntax hexLiteralExpression:
                    value = hexLiteralExpression.Value.ToString();
                    return true;

                case JassStringLiteralExpressionSyntax stringLiteralExpression:
                    value = stringLiteralExpression.Value;
                    return true;

                case JassParenthesizedExpressionSyntax parenthesizedExpression:
                    if (TryGetStringExpressionValue(parenthesizedExpression.Expression, out var parenthesizedExpressionValue))
                    {
                        value = parenthesizedExpressionValue;
                        return true;
                    }
                    break;

                case JassCharacterLiteralExpressionSyntax charLiteralExpression:
                    value = charLiteralExpression.Value.ToString();
                    return true;

                case JassNullLiteralExpressionSyntax:
                    value = JassKeyword.Null;
                    return true;
            }

            value = default;
            return false;
        }
    }
}