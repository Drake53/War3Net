// ------------------------------------------------------------------------------
// <copyright file="VJassIfStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIfStatementSyntax : IStatementSyntax
    {
        public VJassIfStatementSyntax(
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
            return other is VJassIfStatementSyntax ifStatement
                && IfClause.Equals(ifStatement.IfClause)
                && ElseIfClauses.SequenceEqual(ifStatement.ElseIfClauses)
                && ElseClause.NullableEquals(ifStatement.ElseClause);
        }

        public override string ToString() => IfClause.ToString();
    }
}