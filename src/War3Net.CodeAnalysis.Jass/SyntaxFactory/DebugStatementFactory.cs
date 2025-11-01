// ------------------------------------------------------------------------------
// <copyright file="DebugStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassDebugStatementSyntax DebugStatement(JassStatementSyntax statement)
        {
            // Statement validation
            JassSyntaxFacts.GetDebugStatementKind(statement.SyntaxKind);

            return new JassDebugStatementSyntax(
                Token(JassSyntaxKind.DebugKeyword),
                statement);
        }

        public static JassDebugStatementSyntax DebugStatement(JassSyntaxToken debugToken, JassStatementSyntax statement)
        {
            ThrowHelper.ThrowIfInvalidToken(debugToken, JassSyntaxKind.DebugKeyword);

            // Statement validation
            JassSyntaxFacts.GetDebugStatementKind(statement.SyntaxKind);

            return new JassDebugStatementSyntax(
                debugToken,
                statement);
        }
    }
}