// ------------------------------------------------------------------------------
// <copyright file="RealLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetRealLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return OneOf(
                Try(Token(char.IsDigit).AtLeastOnce().Before(Symbol.Dot)).Then(Token(char.IsDigit).Many()),
                Symbol.Dot.Then(Token(char.IsDigit).AtLeastOnce()))
                .MapWithInput((s, _) => s.ToString())
                .AsToken(triviaParser, JassSyntaxKind.RealLiteralToken)
                .Map(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token))
                .Labelled("real literal");
        }
    }
}