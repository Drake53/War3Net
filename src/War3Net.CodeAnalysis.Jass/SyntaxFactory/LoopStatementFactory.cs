// ------------------------------------------------------------------------------
// <copyright file="LoopStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax LoopStatement(params NewStatementSyntax[] statements)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new LoopStatementSyntax(
                        Token(SyntaxTokenType.LoopKeyword),
                        Newlines(),
                        StatementList(statements),
                        Token(SyntaxTokenType.EndloopKeyword))),
                Newlines(2));
        }
    }
}