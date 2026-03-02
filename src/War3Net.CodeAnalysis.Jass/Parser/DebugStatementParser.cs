using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetDebugStatementParser(
            Parser<char, JassStatementSyntax> setStatementParser,
            Parser<char, JassStatementSyntax> callStatementParser,
            Parser<char, JassStatementSyntax> ifStatementParser,
            Parser<char, JassStatementSyntax> loopStatementParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (debugToken, statement) => (JassStatementSyntax)new JassDebugStatementSyntax(
                    debugToken,
                    statement),
                Keyword.Debug.AsToken(triviaParser, JassSyntaxKind.DebugKeyword),
                OneOf(
                    setStatementParser,
                    callStatementParser,
                    ifStatementParser,
                    loopStatementParser));
        }
    }
}