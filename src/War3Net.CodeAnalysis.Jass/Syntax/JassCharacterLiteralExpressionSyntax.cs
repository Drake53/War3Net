// ------------------------------------------------------------------------------
// <copyright file="JassCharacterLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete]
    public class JassCharacterLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassCharacterLiteralExpressionSyntax(char value)
        {
            Value = value;
        }

        public char Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassCharacterLiteralExpressionSyntax characterLiteralExpression
                && Value == characterLiteralExpression.Value;
        }

        public override string ToString() => $"{JassSymbol.SingleQuoteChar}{Value}{JassSymbol.SingleQuoteChar}";
    }
}