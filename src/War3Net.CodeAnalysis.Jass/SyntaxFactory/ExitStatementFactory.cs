// ------------------------------------------------------------------------------
// <copyright file="ExitStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax ExitStatement(NewExpressionSyntax expression)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new ExitStatementSyntax(
                        Token(SyntaxTokenType.ExitwhenKeyword),
                        expression)),
                Newlines());
        }
    }
}