// ------------------------------------------------------------------------------
// <copyright file="JassOctalLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassOctalLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassOctalLiteralExpressionSyntax(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassOctalLiteralExpressionSyntax octalLiteralExpression
                && Value == octalLiteralExpression.Value;
        }

        public override string ToString() => Value == 0 ? "0" : $"0{Convert.ToString(Value, 8)}";
    }
}