// ------------------------------------------------------------------------------
// <copyright file="VJassExtendsClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassExtendsClauseSyntax : VJassSyntaxNode
    {
        internal VJassExtendsClauseSyntax(
            VJassSyntaxToken extendsToken,
            VJassIdentifierNameSyntax identifierName)
        {
            ExtendsToken = extendsToken;
            IdentifierName = identifierName;
        }

        public VJassSyntaxToken ExtendsToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassExtendsClauseSyntax extendsClause
                && IdentifierName.IsEquivalentTo(extendsClause.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            ExtendsToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => $"{ExtendsToken} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => ExtendsToken;

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassExtendsClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassExtendsClauseSyntax(
                newToken,
                IdentifierName);
        }

        protected internal override VJassExtendsClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassExtendsClauseSyntax(
                ExtendsToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}