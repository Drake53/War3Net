using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassArgumentListSyntax> GetArgumentListParser(
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassExpressionSyntax> expressionParser)
        {
            return Map(
                (openParenToken, argumentList, closeParenToken) => new JassArgumentListSyntax(
                    openParenToken,
                    argumentList,
                    closeParenToken),
                Symbol.OpenParen.AsToken(triviaParser, JassSyntaxKind.OpenParenToken, JassSymbol.OpenParen),
                expressionParser.SeparatedList(Symbol.Comma.AsToken(triviaParser, JassSyntaxKind.CommaToken, JassSymbol.Comma)),
                Symbol.CloseParen.AsToken(triviaParser, JassSyntaxKind.CloseParenToken, JassSymbol.CloseParen));
        }
    }
}