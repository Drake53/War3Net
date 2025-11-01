// ------------------------------------------------------------------------------
// <copyright file="JassCharacterLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCharacterLiteralExpressionSyntax : IExpressionSyntax, IJassSyntaxToken
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

        public override string ToString() => $"{JassSymbol.Apostrophe}{Value}{JassSymbol.Apostrophe}";
    }
}