// ------------------------------------------------------------------------------
// <copyright file="NullLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetNullLiteralExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Keyword.Null.AsToken(triviaParser, VJassSyntaxKind.NullKeyword)
                .Map(token => (VJassExpressionSyntax)new VJassLiteralExpressionSyntax(token))
                .Labelled("null literal");
        }
    }
}