// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfMemberDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfMemberDeclarationSyntax : VJassMemberDeclarationSyntax
    {
        internal VJassStaticIfMemberDeclarationSyntax(
            VJassMemberDeclarationStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassMemberDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassMemberDeclarationElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassMemberDeclarationStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassMemberDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassMemberDeclarationElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfMemberDeclarationSyntax staticIfMemberDeclaration
                && StaticIfClause.IsEquivalentTo(staticIfMemberDeclaration.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfMemberDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfMemberDeclaration.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override string ToString() => StaticIfClause.ToString();

        public override VJassSyntaxToken GetFirstToken() => StaticIfClause.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override VJassStaticIfMemberDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfMemberDeclarationSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfMemberDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfMemberDeclarationSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}