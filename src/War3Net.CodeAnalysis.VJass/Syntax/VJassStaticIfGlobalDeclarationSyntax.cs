// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfGlobalDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfGlobalDeclarationSyntax : IGlobalDeclarationSyntax
    {
        public VJassStaticIfGlobalDeclarationSyntax(
            VJassGlobalDeclarationIfClauseSyntax ifClause,
            ImmutableArray<VJassGlobalDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassGlobalDeclarationElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassGlobalDeclarationIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassGlobalDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassGlobalDeclarationElseClauseSyntax? ElseClause { get; }

        public bool Equals(IGlobalDeclarationSyntax? other)
        {
            return other is VJassStaticIfGlobalDeclarationSyntax staticIfGlobalDeclaration
                && IfClause.Equals(staticIfGlobalDeclaration.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfGlobalDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfGlobalDeclaration.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}