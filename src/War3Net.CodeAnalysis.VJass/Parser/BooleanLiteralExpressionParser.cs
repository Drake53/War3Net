// ------------------------------------------------------------------------------
// <copyright file="BooleanLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetBooleanLiteralExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return OneOf(
                Keyword.True.AsToken(triviaParser, VJassSyntaxKind.TrueKeyword),
                Keyword.False.AsToken(triviaParser, VJassSyntaxKind.FalseKeyword))
                .Select(token => (VJassExpressionSyntax)new VJassLiteralExpressionSyntax(token));
        }
    }
}