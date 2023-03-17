// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassElseIfClauseSyntax ElseIfClause(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, params JassStatementSyntax[] statements)
        {
            return new JassElseIfClauseSyntax(
                elseIfClauseDeclarator,
                statements.ToImmutableArray());
        }

        public static JassElseIfClauseSyntax ElseIfClause(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, IEnumerable<JassStatementSyntax> statements)
        {
            return new JassElseIfClauseSyntax(
                elseIfClauseDeclarator,
                statements.ToImmutableArray());
        }

        public static JassElseIfClauseSyntax ElseIfClause(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator, ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassElseIfClauseSyntax(
                elseIfClauseDeclarator,
                statements);
        }

        public static JassElseIfClauseSyntax ElseIfClause(JassExpressionSyntax condition, params JassStatementSyntax[] statements)
        {
            return new JassElseIfClauseSyntax(
                ElseIfClauseDeclarator(condition),
                statements.ToImmutableArray());
        }

        public static JassElseIfClauseSyntax ElseIfClause(JassExpressionSyntax condition, IEnumerable<JassStatementSyntax> statements)
        {
            return new JassElseIfClauseSyntax(
                ElseIfClauseDeclarator(condition),
                statements.ToImmutableArray());
        }

        public static JassElseIfClauseSyntax ElseIfClause(JassExpressionSyntax condition, ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassElseIfClauseSyntax(
                ElseIfClauseDeclarator(condition),
                statements);
        }
    }
}