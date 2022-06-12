// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfTopLevelDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfTopLevelDeclarationSyntax : VJassTopLevelDeclarationSyntax
    {
        internal VJassStaticIfTopLevelDeclarationSyntax(
            VJassTopLevelDeclarationStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassTopLevelDeclarationElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassTopLevelDeclarationStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassTopLevelDeclarationElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfTopLevelDeclarationSyntax staticIfTopLevelDeclaration
                && StaticIfClause.IsEquivalentTo(staticIfTopLevelDeclaration.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfTopLevelDeclaration.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(staticIfTopLevelDeclaration.ElseClause);
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

        protected internal override VJassStaticIfTopLevelDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfTopLevelDeclarationSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfTopLevelDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfTopLevelDeclarationSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}