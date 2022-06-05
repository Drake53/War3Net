// ------------------------------------------------------------------------------
// <copyright file="VJassElementAccessExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassElementAccessExpressionSyntax : IExpressionSyntax
    {
        public VJassElementAccessExpressionSyntax(
            IExpressionSyntax expression,
            IExpressionSyntax indexer)
        {
            Expression = expression;
            Indexer = indexer;
        }

        public IExpressionSyntax Expression { get; }

        public IExpressionSyntax Indexer { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassElementAccessExpressionSyntax elementAccessExpression
                && Expression.Equals(elementAccessExpression.Expression)
                && Indexer.Equals(elementAccessExpression.Indexer);
        }

        public override string ToString() => $"{Expression}{VJassSymbol.LeftSquareBracket}{Indexer}{VJassSymbol.RightSquareBracket}";
    }
}