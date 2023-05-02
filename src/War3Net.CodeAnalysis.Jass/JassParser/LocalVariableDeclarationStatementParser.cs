// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetLocalVariableDeclarationStatementParser(
            Parser<char, JassVariableOrArrayDeclaratorSyntax> variableOrArrayDeclaratorParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (localToken, declarator, trailingTrivia) => (JassStatementSyntax)new JassLocalVariableDeclarationStatementSyntax(
                    localToken,
                    declarator.AppendTrivia(trailingTrivia)),
                Keyword.Local.AsToken(triviaParser, JassSyntaxKind.LocalKeyword),
                variableOrArrayDeclaratorParser,
                trailingTriviaParser);
        }
    }
}