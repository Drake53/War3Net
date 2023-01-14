// ------------------------------------------------------------------------------
// <copyright file="IfStatementFactory.cs" company="Drake53">
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
        public static JassIfStatementSyntax IfStatement(JassIfClauseSyntax ifClause)
        {
            return new JassIfStatementSyntax(
                ifClause,
                ImmutableArray<JassElseIfClauseSyntax>.Empty,
                null,
                Token(JassSyntaxKind.EndIfKeyword));
        }

        public static JassIfStatementSyntax IfStatement(JassExpressionSyntax condition, params JassStatementSyntax[] body)
        {
            return new JassIfStatementSyntax(
                IfClause(condition, body),
                ImmutableArray<JassElseIfClauseSyntax>.Empty,
                null,
                Token(JassSyntaxKind.EndIfKeyword));
        }

        public static JassIfStatementSyntax IfStatement(JassIfClauseSyntax ifClause, JassElseClauseSyntax? elseClause)
        {
            return new JassIfStatementSyntax(
                ifClause,
                ImmutableArray<JassElseIfClauseSyntax>.Empty,
                elseClause,
                Token(JassSyntaxKind.EndIfKeyword));
        }

        public static JassIfStatementSyntax IfStatement(JassIfClauseSyntax ifClause, params JassElseIfClauseSyntax[] elseIfClauses)
        {
            return new JassIfStatementSyntax(
                ifClause,
                elseIfClauses.ToImmutableArray(),
                null,
                Token(JassSyntaxKind.EndIfKeyword));
        }

        public static JassIfStatementSyntax IfStatement(JassIfClauseSyntax ifClause, IEnumerable<JassElseIfClauseSyntax> elseIfClauses, JassElseClauseSyntax? elseClause)
        {
            return new JassIfStatementSyntax(
                ifClause,
                elseIfClauses.ToImmutableArray(),
                elseClause,
                Token(JassSyntaxKind.EndIfKeyword));
        }
    }
}