using Pidgin;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassSyntaxToken> GetUnaryOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return OneOf(
                GetUnaryPlusOperatorParser(triviaParser),
                GetUnaryMinusOperatorParser(triviaParser),
                GetUnaryNotOperatorParser(triviaParser));
        }

        internal static Parser<char, JassSyntaxToken> GetUnaryPlusOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Plus.AsToken(triviaParser, JassSyntaxKind.PlusToken, JassSymbol.Plus);
        }

        internal static Parser<char, JassSyntaxToken> GetUnaryMinusOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Minus.AsToken(triviaParser, JassSyntaxKind.MinusToken, JassSymbol.Minus);
        }

        internal static Parser<char, JassSyntaxToken> GetUnaryNotOperatorParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Not.AsToken(triviaParser, JassSyntaxKind.NotKeyword);
        }
    }
}