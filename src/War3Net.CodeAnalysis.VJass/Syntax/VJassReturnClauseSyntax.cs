// ------------------------------------------------------------------------------
// <copyright file="VJassReturnClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassReturnClauseSyntax : VJassSyntaxNode
    {
        internal VJassReturnClauseSyntax(
            VJassSyntaxToken returnsToken,
            VJassTypeSyntax returnType)
        {
            ReturnsToken = returnsToken;
            ReturnType = returnType;
        }

        public VJassSyntaxToken ReturnsToken { get; }

        public VJassTypeSyntax ReturnType { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassReturnClauseSyntax returnClause
                && ReturnType.IsEquivalentTo(returnClause.ReturnType);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnsToken.WriteTo(writer);
            ReturnType.WriteTo(writer);
        }

        public override string ToString() => $"{ReturnsToken} {ReturnType}";

        public override VJassSyntaxToken GetFirstToken() => ReturnsToken;

        public override VJassSyntaxToken GetLastToken() => ReturnType.GetLastToken();

        protected internal override VJassReturnClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassReturnClauseSyntax(
                newToken,
                ReturnType);
        }

        protected internal override VJassReturnClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassReturnClauseSyntax(
                ReturnsToken,
                ReturnType.ReplaceLastToken(newToken));
        }
    }
}