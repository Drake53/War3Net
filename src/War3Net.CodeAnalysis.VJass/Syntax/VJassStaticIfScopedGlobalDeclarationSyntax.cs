// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfScopedGlobalDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfScopedGlobalDeclarationSyntax : IScopedGlobalDeclarationSyntax
    {
        public VJassStaticIfScopedGlobalDeclarationSyntax(
            VJassScopedGlobalDeclarationIfClauseSyntax ifClause,
            ImmutableArray<VJassScopedGlobalDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassScopedGlobalDeclarationElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassScopedGlobalDeclarationIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassScopedGlobalDeclarationElseClauseSyntax? ElseClause { get; }

        public bool Equals(IScopedGlobalDeclarationSyntax? other)
        {
            return other is VJassStaticIfScopedGlobalDeclarationSyntax staticIfScopedGlobalDeclaration
                && IfClause.Equals(staticIfScopedGlobalDeclaration.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfScopedGlobalDeclaration.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfScopedGlobalDeclaration.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}