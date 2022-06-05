// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfScopedDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfScopedDeclarationSyntax : IScopedDeclarationSyntax
    {
        public VJassStaticIfScopedDeclarationSyntax(
            VJassScopedDeclarationIfClauseSyntax ifClause,
            ImmutableArray<VJassScopedDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassScopedDeclarationElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassScopedDeclarationIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassScopedDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassScopedDeclarationElseClauseSyntax? ElseClause { get; }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassStaticIfScopedDeclarationSyntax staticIfScopedDeclaration
                && IfClause.Equals(staticIfScopedDeclaration.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfScopedDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfScopedDeclaration.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}