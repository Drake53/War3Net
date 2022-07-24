// ------------------------------------------------------------------------------
// <copyright file="VJassEqualsValueClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassEqualsValueClauseSyntax : VJassSyntaxNode
    {
        internal VJassEqualsValueClauseSyntax(
            VJassSyntaxToken equalsToken,
            VJassExpressionSyntax expression)
        {
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public VJassSyntaxToken EqualsToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassEqualsValueClauseSyntax equalsValueClause
                && Expression.IsEquivalentTo(equalsValueClause.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            EqualsToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            EqualsToken.ProcessTo(writer, context);
            Expression.ProcessTo(writer, context);
        }

        public override string ToString() => $"{EqualsToken} {Expression}";

        public override VJassSyntaxToken GetFirstToken() => EqualsToken;

        public override VJassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override VJassEqualsValueClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassEqualsValueClauseSyntax(
                newToken,
                Expression);
        }

        protected internal override VJassEqualsValueClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassEqualsValueClauseSyntax(
                EqualsToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}