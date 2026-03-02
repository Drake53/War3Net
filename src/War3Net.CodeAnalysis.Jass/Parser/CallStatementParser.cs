using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetCallStatementParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassArgumentListSyntax> argumentListParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (callToken, identifierName, argumentList, trailingTrivia) => (JassStatementSyntax)new JassCallStatementSyntax(
                    callToken,
                    identifierName,
                    argumentList.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Call.AsToken(triviaParser, JassSyntaxKind.CallKeyword),
                identifierNameParser,
                argumentListParser,
                trailingTriviaParser);
        }
    }
}