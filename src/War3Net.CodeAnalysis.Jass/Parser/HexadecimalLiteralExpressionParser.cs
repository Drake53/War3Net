// ------------------------------------------------------------------------------
// <copyright file="HexadecimalLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, JassExpressionSyntax> GetHexadecimalLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Symbol.Dollar.Or(Try(Symbol.Zero.Then(Symbol.X))).Then(UnsignedInt(16))
                .MapWithInput((s, _) => s.ToString())
                .AsToken(triviaParser, JassSyntaxKind.HexadecimalLiteralToken)
                .Map(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token))
                .Labelled("hexadecimal literal");
        }
    }
}