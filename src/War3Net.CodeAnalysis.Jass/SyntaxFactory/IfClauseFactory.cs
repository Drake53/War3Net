// ------------------------------------------------------------------------------
// <copyright file="IfClauseFactory.cs" company="Drake53">
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
        public static JassIfClauseSyntax IfClause(JassExpressionSyntax condition, params JassStatementSyntax[] statements)
        {
            return new JassIfClauseSyntax(
                IfClauseDeclarator(condition),
                statements.ToImmutableArray());
        }

        public static JassIfClauseSyntax IfClause(JassExpressionSyntax condition, IEnumerable<JassStatementSyntax> statements)
        {
            return new JassIfClauseSyntax(
                IfClauseDeclarator(condition),
                statements.ToImmutableArray());
        }

        public static JassIfClauseSyntax IfClause(JassExpressionSyntax condition, ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassIfClauseSyntax(
                IfClauseDeclarator(condition),
                statements);
        }

        public static JassIfClauseSyntax IfClause(JassIfClauseDeclaratorSyntax ifClauseDeclarator, params JassStatementSyntax[] statements)
        {
            return new JassIfClauseSyntax(
                ifClauseDeclarator,
                statements.ToImmutableArray());
        }

        public static JassIfClauseSyntax IfClause(JassIfClauseDeclaratorSyntax ifClauseDeclarator, IEnumerable<JassStatementSyntax> statements)
        {
            return new JassIfClauseSyntax(
                ifClauseDeclarator,
                statements.ToImmutableArray());
        }

        public static JassIfClauseSyntax IfClause(JassIfClauseDeclaratorSyntax ifClauseDeclarator, ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassIfClauseSyntax(
                ifClauseDeclarator,
                statements);
        }
    }
}