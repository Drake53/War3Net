using Pidgin;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetDecimalLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Try(UnsignedInt(10))
                .MapWithInput((s, _) => s.ToString())
                .AsToken(triviaParser, JassSyntaxKind.DecimalLiteralToken)
                .Map(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token))
                .Labelled("decimal literal");
        }
    }
}