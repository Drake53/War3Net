using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassElementAccessClauseSyntax> GetElementAccessClauseParser(
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassExpressionSyntax> expressionParser)
        {
            return Map(
                (openBracketToken, expression, closeBracketToken) => new JassElementAccessClauseSyntax(
                    openBracketToken,
                    expression,
                    closeBracketToken),
                Symbol.OpenBracket.AsToken(triviaParser, JassSyntaxKind.OpenBracketToken, JassSymbol.OpenBracket),
                expressionParser,
                Symbol.CloseBracket.AsToken(triviaParser, JassSyntaxKind.CloseBracketToken, JassSymbol.CloseBracket));
        }
    }
}