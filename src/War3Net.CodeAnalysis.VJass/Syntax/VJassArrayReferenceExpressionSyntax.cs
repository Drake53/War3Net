// ------------------------------------------------------------------------------
// <copyright file="VJassArrayReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassArrayReferenceExpressionSyntax : IExpressionSyntax
    {
        public VJassArrayReferenceExpressionSyntax(
            VJassIdentifierNameSyntax identifierName,
            IExpressionSyntax indexer)
        {
            IdentifierName = identifierName;
            Indexer = indexer;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public IExpressionSyntax Indexer { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassArrayReferenceExpressionSyntax arrayReferenceExpression
                && IdentifierName.Equals(arrayReferenceExpression.IdentifierName)
                && Indexer.Equals(arrayReferenceExpression.Indexer);
        }

        public override string ToString() => $"{IdentifierName}{VJassSymbol.LeftSquareBracket}{Indexer}{VJassSymbol.RightSquareBracket}";
    }
}