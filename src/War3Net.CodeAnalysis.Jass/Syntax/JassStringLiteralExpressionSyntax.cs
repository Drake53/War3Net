// ------------------------------------------------------------------------------
// <copyright file="JassStringLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassStringLiteralExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
    {
        public JassStringLiteralExpressionSyntax(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is JassStringLiteralExpressionSyntax stringLiteralExpression
                && string.Equals(Value, stringLiteralExpression.Value, StringComparison.Ordinal);
        }

        public override string ToString() => $"{JassSymbol.QuotationMark}{Value}{JassSymbol.QuotationMark}";
    }
}