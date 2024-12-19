// ------------------------------------------------------------------------------
// <copyright file="JassRealLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassRealLiteralExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
    {
#if true
        public JassRealLiteralExpressionSyntax(string intPart, string fracPart)
        {
            IntPart = intPart;
            FracPart = fracPart;
        }

        public string IntPart { get; init; }

        public string FracPart { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            if (other is JassRealLiteralExpressionSyntax realLiteralExpression && FracPart.Length == realLiteralExpression.FracPart.Length)
            {
                var left = float.Parse(ToString(), CultureInfo.InvariantCulture);
                var right = float.Parse(realLiteralExpression.ToString(), CultureInfo.InvariantCulture);

                if (left == right)
                {
                    return true;
                }

                var difference = MathF.Abs(left - right);
                var maxDifference = MathF.Pow(10, -FracPart.Length) * 1.01f;
                return difference <= maxDifference;
            }

            return false;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(FracPart)
                ? $"{IntPart}{JassSymbol.FullStop}"
                : $"{IntPart}{JassSymbol.FullStop}{FracPart}";
        }
#else
        public JassRealLiteralExpressionSyntax(float value)
        {
            Value = value;
        }

        public float Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassRealLiteralExpressionSyntax realLiteralExpression
                && Value == realLiteralExpression.Value;
        }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
#endif
    }
}