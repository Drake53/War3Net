// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfTopLevelDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfTopLevelDeclarationSyntax : ITopLevelDeclarationSyntax
    {
        public VJassStaticIfTopLevelDeclarationSyntax(
            VJassTopLevelDeclarationIfClauseSyntax ifClause,
            ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassTopLevelDeclarationElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassTopLevelDeclarationIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassTopLevelDeclarationElseClauseSyntax? ElseClause { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassStaticIfTopLevelDeclarationSyntax staticIfTopLevelDeclaration
                && IfClause.Equals(staticIfTopLevelDeclaration.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfTopLevelDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfTopLevelDeclaration.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}