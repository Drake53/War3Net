using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassReturnClauseSyntax> GetReturnClauseParser(
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (returnsToken, type) => new JassReturnClauseSyntax(
                    returnsToken,
                    type),
                Keyword.Returns.AsToken(triviaParser, JassSyntaxKind.ReturnsKeyword),
                typeParser);
        }
    }
}