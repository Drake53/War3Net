// ------------------------------------------------------------------------------
// <copyright file="DecimalLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Globalization;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetDecimalLiteralExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (value, trivia) => (VJassExpressionSyntax)new VJassLiteralExpressionSyntax(
                    new VJassSyntaxToken(VJassSyntaxKind.DecimalLiteralToken, value.ToString(CultureInfo.InvariantCulture), trivia)),
                Try(UnsignedInt(10)),
                triviaParser)
                .Labelled("decimal literal");
        }
    }
}