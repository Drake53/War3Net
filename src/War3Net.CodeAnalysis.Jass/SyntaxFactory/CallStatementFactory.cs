// ------------------------------------------------------------------------------
// <copyright file="CallStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassCallStatementSyntax CallStatement(JassIdentifierNameSyntax identifierName, JassArgumentListSyntax argumentList)
        {
            return new JassCallStatementSyntax(
                Token(JassSyntaxKind.CallKeyword),
                identifierName,
                argumentList);
        }

        public static JassCallStatementSyntax CallStatement(JassIdentifierNameSyntax identifierName, params JassExpressionSyntax[] arguments)
        {
            return new JassCallStatementSyntax(
                Token(JassSyntaxKind.CallKeyword),
                identifierName,
                ArgumentList(arguments));
        }

        public static JassCallStatementSyntax CallStatement(string name, JassArgumentListSyntax argumentList)
        {
            return new JassCallStatementSyntax(
                Token(JassSyntaxKind.CallKeyword),
                ParseIdentifierName(name),
                argumentList);
        }

        public static JassCallStatementSyntax CallStatement(string name, params JassExpressionSyntax[] arguments)
        {
            return new JassCallStatementSyntax(
                Token(JassSyntaxKind.CallKeyword),
                ParseIdentifierName(name),
                ArgumentList(arguments));
        }

        public static JassCallStatementSyntax CallStatement(JassSyntaxToken callToken, JassIdentifierNameSyntax identifierName, JassArgumentListSyntax argumentList)
        {
            ThrowHelper.ThrowIfInvalidToken(callToken, JassSyntaxKind.CallKeyword);

            return new JassCallStatementSyntax(
                callToken,
                identifierName,
                argumentList);
        }
    }
}