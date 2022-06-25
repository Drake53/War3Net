// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationStaticIfClauseSyntax.cs" company="Drake53">
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
    public class VJassScopedDeclarationStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedDeclarationStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassScopedDeclarationSyntax> declarations)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            Declarations = declarations;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassScopedDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedDeclarationStaticIfClauseSyntax scopedDeclarationStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(scopedDeclarationStaticIfClause.StaticIfClauseDeclarator)
                && Declarations.IsEquivalentTo(scopedDeclarationStaticIfClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            StaticIfClauseDeclarator.ProcessTo(writer, context);
            Declarations.ProcessTo(writer, context);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : Declarations[^1].GetLastToken();

        protected internal override VJassScopedDeclarationStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                Declarations);
        }

        protected internal override VJassScopedDeclarationStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassScopedDeclarationStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                Declarations);
        }
    }
}