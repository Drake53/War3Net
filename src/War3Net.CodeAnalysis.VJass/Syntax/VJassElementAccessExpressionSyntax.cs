// ------------------------------------------------------------------------------
// <copyright file="VJassElementAccessExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassElementAccessExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassElementAccessExpressionSyntax(
            VJassExpressionSyntax expression,
            VJassSyntaxToken leftBracketToken,
            VJassExpressionSyntax indexer,
            VJassSyntaxToken rightBracketToken)
        {
            Expression = expression;
            LeftBracketToken = leftBracketToken;
            Indexer = indexer;
            RightBracketToken = rightBracketToken;
        }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken LeftBracketToken { get; }

        public VJassExpressionSyntax Indexer { get; }

        public VJassSyntaxToken RightBracketToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassElementAccessExpressionSyntax elementAccessExpression
                && Expression.IsEquivalentTo(elementAccessExpression.Expression)
                && Indexer.IsEquivalentTo(elementAccessExpression.Indexer);
        }

        public override void WriteTo(TextWriter writer)
        {
            Expression.WriteTo(writer);
            LeftBracketToken.WriteTo(writer);
            Indexer.WriteTo(writer);
            RightBracketToken.WriteTo(writer);
        }

        public override string ToString() => $"{Expression}{LeftBracketToken}{Indexer}{RightBracketToken}";

        public override VJassSyntaxToken GetFirstToken() => Expression.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => RightBracketToken;

        protected internal override VJassElementAccessExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassElementAccessExpressionSyntax(
                Expression.ReplaceFirstToken(newToken),
                LeftBracketToken,
                Indexer,
                RightBracketToken);
        }

        protected internal override VJassElementAccessExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassElementAccessExpressionSyntax(
                Expression,
                LeftBracketToken,
                Indexer,
                newToken);
        }
    }
}