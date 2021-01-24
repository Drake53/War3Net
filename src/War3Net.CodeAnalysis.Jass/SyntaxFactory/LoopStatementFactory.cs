// ------------------------------------------------------------------------------
// <copyright file="LoopStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassLoopStatementSyntax LoopStatement(JassStatementListSyntax body)
        {
            return new JassLoopStatementSyntax(body);
        }

        public static JassLoopStatementSyntax LoopStatement(params IStatementSyntax[] body)
        {
            return new JassLoopStatementSyntax(StatementList(body));
        }
    }
}