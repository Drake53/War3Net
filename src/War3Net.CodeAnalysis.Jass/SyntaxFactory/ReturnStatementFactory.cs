// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax ReturnStatement()
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new ReturnStatementSyntax(
                        Token(SyntaxTokenType.ReturnKeyword),
                        Empty())),
                Newlines());
        }

        public static NewStatementSyntax ReturnStatement(NewExpressionSyntax expression)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new ReturnStatementSyntax(
                        Token(SyntaxTokenType.ReturnKeyword),
                        expression)),
                Newlines());
        }
    }
}