// ------------------------------------------------------------------------------
// <copyright file="JassDecimalLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDecimalLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassDecimalLiteralExpressionSyntax(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassDecimalLiteralExpressionSyntax decimalLiteralExpression
                && Value == decimalLiteralExpression.Value;
        }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
    }
}