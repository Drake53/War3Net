// ------------------------------------------------------------------------------
// <copyright file="ElseClauseFactory.cs" company="Drake53">
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
        public static JassElseClauseSyntax ElseClause(params JassStatementSyntax[] statements)
        {
            return new JassElseClauseSyntax(
                Token(JassSyntaxKind.ElseKeyword),
                statements.ToImmutableArray());
        }

        public static JassElseClauseSyntax ElseClause(IEnumerable<JassStatementSyntax> statements)
        {
            return new JassElseClauseSyntax(
                Token(JassSyntaxKind.ElseKeyword),
                statements.ToImmutableArray());
        }

        public static JassElseClauseSyntax ElseClause(ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassElseClauseSyntax(
                Token(JassSyntaxKind.ElseKeyword),
                statements);
        }
    }
}