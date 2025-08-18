// ------------------------------------------------------------------------------
// <copyright file="JassDecimalLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDecimalLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassDecimalLiteralExpressionSyntax(long value)
        {
            if (value < Int32.MinValue || value-1 > Int32.MaxValue)
            {
                //workaround to allow -2147483648 to be wrapped as new JassUnaryExpressionSyntax(UnaryOperatorType.Minus, new JassDecimalLiteralExpressionSyntax(2147483648))
                throw new ArgumentException();
            }

            Value = value;
        }

        public long Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassDecimalLiteralExpressionSyntax decimalLiteralExpression
                && Value == decimalLiteralExpression.Value;
        }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}