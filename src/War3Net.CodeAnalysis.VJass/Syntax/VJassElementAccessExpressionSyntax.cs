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
            VJassSyntaxToken openBracketToken,
            VJassExpressionSyntax indexer,
            VJassSyntaxToken closeBracketToken)
        {
            Expression = expression;
            OpenBracketToken = openBracketToken;
            Indexer = indexer;
            CloseBracketToken = closeBracketToken;
        }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken OpenBracketToken { get; }

        public VJassExpressionSyntax Indexer { get; }

        public VJassSyntaxToken CloseBracketToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassElementAccessExpressionSyntax elementAccessExpression
                && Expression.IsEquivalentTo(elementAccessExpression.Expression)
                && Indexer.IsEquivalentTo(elementAccessExpression.Indexer);
        }

        public override void WriteTo(TextWriter writer)
        {
            Expression.WriteTo(writer);
            OpenBracketToken.WriteTo(writer);
            Indexer.WriteTo(writer);
            CloseBracketToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Expression.ProcessTo(writer, context);
            OpenBracketToken.ProcessTo(writer, context);
            Indexer.ProcessTo(writer, context);
            CloseBracketToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Expression}{OpenBracketToken}{Indexer}{CloseBracketToken}";

        public override VJassSyntaxToken GetFirstToken() => Expression.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => CloseBracketToken;

        protected internal override VJassElementAccessExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassElementAccessExpressionSyntax(
                Expression.ReplaceFirstToken(newToken),
                OpenBracketToken,
                Indexer,
                CloseBracketToken);
        }

        protected internal override VJassElementAccessExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassElementAccessExpressionSyntax(
                Expression,
                OpenBracketToken,
                Indexer,
                newToken);
        }
    }
}