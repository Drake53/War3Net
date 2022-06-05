// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfStatementSyntax.cs" company="Drake53">
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
    public class VJassStaticIfStatementSyntax : IStatementSyntax
    {
        public VJassStaticIfStatementSyntax(
            VJassStatementIfClauseSyntax ifClause,
            ImmutableArray<VJassStatementElseIfClauseSyntax> elseIfClauses,
            VJassStatementElseClauseSyntax? elseClause)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public VJassStatementIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassStatementElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassStatementElseClauseSyntax? ElseClause { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassStaticIfStatementSyntax staticIfStatement
                && IfClause.Equals(staticIfStatement.IfClause)
                && ElseIfClauses.SequenceEqual(staticIfStatement.ElseIfClauses)
                && ElseClause.NullableEquals(staticIfStatement.ElseClause);
        }

        public override string ToString() => $"{VJassKeyword.Static} {IfClause}";
    }
}