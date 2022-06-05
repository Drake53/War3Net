// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfMemberDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStaticIfMemberDeclarationSyntax : IMemberDeclarationSyntax
    {
        public VJassStaticIfMemberDeclarationSyntax(
            VJassMemberDeclarationIfClauseSyntax ifClause,
            ImmutableArray<VJassMemberDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassMemberDeclarationElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassMemberDeclarationIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassMemberDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassMemberDeclarationElseClauseSyntax? ElseClause { get; }

        public bool Equals(IMemberDeclarationSyntax? other)
        {
            return other is VJassStaticIfMemberDeclarationSyntax staticIfMemberDeclaration
                && IfClause.Equals(staticIfMemberDeclaration.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfMemberDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfMemberDeclaration.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}