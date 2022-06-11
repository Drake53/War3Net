// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationStaticIfClauseSyntax.cs" company="Drake53">
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
    public class VJassTopLevelDeclarationStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassTopLevelDeclarationStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassTopLevelDeclarationSyntax> declarations)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            Declarations = declarations;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassTopLevelDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTopLevelDeclarationStaticIfClauseSyntax topLevelDeclarationStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(topLevelDeclarationStaticIfClause.StaticIfClauseDeclarator)
                && Declarations.IsEquivalentTo(topLevelDeclarationStaticIfClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : Declarations[^1].GetLastToken();

        protected internal override VJassTopLevelDeclarationStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTopLevelDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                Declarations);
        }

        protected internal override VJassTopLevelDeclarationStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassTopLevelDeclarationStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassTopLevelDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                Declarations);
        }
    }
}