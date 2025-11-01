// ------------------------------------------------------------------------------
// <copyright file="FourCCLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetFourCCLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.SingleQuote.Then(AnyCharExcept(JassSymbol.SingleQuoteChar).ManyString()).Before(Symbol.SingleQuote)
                .Assert(value => value.IsJassRawcode())
                .MapWithInput((s, _) => s.ToString())
                .AsToken(triviaParser, JassSyntaxKind.FourCCLiteralToken)
                .Map(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token))
                .Labelled("fourCC literal");
        }
    }
}