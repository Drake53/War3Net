// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassScopedDeclarationSyntax> declarations)
        {
            ElseToken = elseToken;
            Declarations = declarations;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassScopedDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedDeclarationElseClauseSyntax scopedDeclarationElseClause
                && Declarations.IsEquivalentTo(scopedDeclarationElseClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? ElseToken : Declarations[^1].GetLastToken();

        protected internal override VJassScopedDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedDeclarationElseClauseSyntax(
                newToken,
                Declarations);
        }

        protected internal override VJassScopedDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassScopedDeclarationElseClauseSyntax(
                    ElseToken,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedDeclarationElseClauseSyntax(
                newToken,
                Declarations);
        }
    }
}