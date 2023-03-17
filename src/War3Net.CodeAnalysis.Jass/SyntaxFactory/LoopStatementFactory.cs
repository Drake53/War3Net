// ------------------------------------------------------------------------------
// <copyright file="LoopStatementFactory.cs" company="Drake53">
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
        public static JassLoopStatementSyntax LoopStatement(params JassStatementSyntax[] statements)
        {
            return new JassLoopStatementSyntax(
                Token(JassSyntaxKind.LoopKeyword),
                statements.ToImmutableArray(),
                Token(JassSyntaxKind.EndLoopKeyword));
        }

        public static JassLoopStatementSyntax LoopStatement(IEnumerable<JassStatementSyntax> statements)
        {
            return new JassLoopStatementSyntax(
                Token(JassSyntaxKind.LoopKeyword),
                statements.ToImmutableArray(),
                Token(JassSyntaxKind.EndLoopKeyword));
        }

        public static JassLoopStatementSyntax LoopStatement(ImmutableArray<JassStatementSyntax> statements)
        {
            return new JassLoopStatementSyntax(
                Token(JassSyntaxKind.LoopKeyword),
                statements,
                Token(JassSyntaxKind.EndLoopKeyword));
        }
    }
}