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
            VJassSyntaxToken equalsSignToken,
            VJassExpressionSyntax expression)
        {
            EqualsSignToken = equalsSignToken;
            Expression = expression;
        }

        public VJassSyntaxToken EqualsSignToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassEqualsValueClauseSyntax equalsValueClause
                && Expression.IsEquivalentTo(equalsValueClause.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            EqualsSignToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override string ToString() => $"{EqualsSignToken} {Expression}";

        public override VJassSyntaxToken GetFirstToken() => EqualsSignToken;

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
                EqualsSignToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}