// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationStaticIfClauseSyntax.cs" company="Drake53">
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
    public class VJassScopedGlobalDeclarationStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedGlobalDeclarationStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassScopedGlobalDeclarationSyntax> globals)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            Globals = globals;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedGlobalDeclarationStaticIfClauseSyntax scopedGlobalDeclarationStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(scopedGlobalDeclarationStaticIfClause.StaticIfClauseDeclarator)
                && Globals.IsEquivalentTo(scopedGlobalDeclarationStaticIfClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : Globals[^1].GetLastToken();

        protected internal override VJassScopedGlobalDeclarationStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                Globals);
        }

        protected internal override VJassScopedGlobalDeclarationStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassScopedGlobalDeclarationStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedGlobalDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                Globals);
        }
    }
}