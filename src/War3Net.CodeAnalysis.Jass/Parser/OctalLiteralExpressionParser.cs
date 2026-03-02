using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetOctalLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Try(Symbol.Zero.Then(UnsignedInt(8)))
                .MapWithInput((s, _) => s.ToString())
                .AsToken(triviaParser, JassSyntaxKind.OctalLiteralToken)
                .Map(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token))
                .Labelled("octal literal");
        }
    }
}