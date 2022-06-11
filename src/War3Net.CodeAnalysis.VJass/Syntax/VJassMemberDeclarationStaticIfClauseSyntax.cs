// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationStaticIfClauseSyntax.cs" company="Drake53">
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
    public class VJassMemberDeclarationStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassMemberDeclarationStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassMemberDeclarationSyntax> memberDeclarations)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            MemberDeclarations = memberDeclarations;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassMemberDeclarationSyntax> MemberDeclarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMemberDeclarationStaticIfClauseSyntax memberDeclarationStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(memberDeclarationStaticIfClause.StaticIfClauseDeclarator)
                && MemberDeclarations.IsEquivalentTo(memberDeclarationStaticIfClause.MemberDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            MemberDeclarations.WriteTo(writer);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => MemberDeclarations.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : MemberDeclarations[^1].GetLastToken();

        protected internal override VJassMemberDeclarationStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                MemberDeclarations);
        }

        protected internal override VJassMemberDeclarationStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!MemberDeclarations.IsEmpty)
            {
                return new VJassMemberDeclarationStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    MemberDeclarations.ReplaceLastItem(MemberDeclarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassMemberDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                MemberDeclarations);
        }
    }
}