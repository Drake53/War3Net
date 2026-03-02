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
                    declarator.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Local.AsToken(triviaParser, JassSyntaxKind.LocalKeyword),
                variableOrArrayDeclaratorParser,
                trailingTriviaParser);
        }
    }
}