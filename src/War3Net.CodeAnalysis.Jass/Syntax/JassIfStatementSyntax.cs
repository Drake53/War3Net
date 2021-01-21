// ------------------------------------------------------------------------------
// <copyright file="JassIfStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIfStatementSyntax : IStatementSyntax
    {
        public JassIfStatementSyntax(IExpressionSyntax condition, JassStatementListSyntax body, ImmutableArray<JassElseIfClauseSyntax> elseIfClauses, JassElseClauseSyntax? elseClause)
        {
            Condition = condition;
            Body = body;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public IExpressionSyntax Condition { get; init; }

        public JassStatementListSyntax Body { get; init; }

        public ImmutableArray<JassElseIfClauseSyntax> ElseIfClauses { get; init; }

        public JassElseClauseSyntax? ElseClause { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassIfStatementSyntax ifStatement
                && Condition.Equals(ifStatement.Condition)
                && Body.Equals(ifStatement.Body)
                && ElseIfClauses.SequenceEqual(ifStatement.ElseIfClauses)
                && ElseClause.NullableEquals(ifStatement.ElseClause);
        }
    }
}