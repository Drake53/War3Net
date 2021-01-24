// ------------------------------------------------------------------------------
// <copyright file="IfStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassIfStatementSyntax IfStatement(IExpressionSyntax condition, JassStatementListSyntax body)
        {
            return new JassIfStatementSyntax(
                condition,
                body,
                ImmutableArray.Create<JassElseIfClauseSyntax>(),
                null);
        }

        public static JassIfStatementSyntax IfStatement(IExpressionSyntax condition, params IStatementSyntax[] body)
        {
            return new JassIfStatementSyntax(
                condition,
                StatementList(body),
                ImmutableArray.Create<JassElseIfClauseSyntax>(),
                null);
        }

        public static JassIfStatementSyntax IfStatement(IExpressionSyntax condition, JassStatementListSyntax body, params JassElseIfClauseSyntax[] elseIfClauses)
        {
            return new JassIfStatementSyntax(
                condition,
                body,
                elseIfClauses.ToImmutableArray(),
                null);
        }
    }
}