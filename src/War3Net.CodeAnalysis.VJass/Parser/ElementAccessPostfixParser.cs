// ------------------------------------------------------------------------------
// <copyright file="ElementAccessPostfixParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, Func<VJassExpressionSyntax, VJassExpressionSyntax>> GetElementAccessPostfixParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassExpressionSyntax> expressionParser)
        {
            return Map<char, VJassSyntaxToken, VJassExpressionSyntax, VJassSyntaxToken, Func<VJassExpressionSyntax, VJassExpressionSyntax>>(
                (openBracketToken, indexer, closeBracketToken) => expression => new VJassElementAccessExpressionSyntax(
                    expression,
                    openBracketToken,
                    indexer,
                    closeBracketToken),
                Symbol.OpenBracket.AsToken(triviaParser, VJassSyntaxKind.OpenBracketToken, VJassSymbol.OpenBracket),
                expressionParser,
                Symbol.CloseBracket.AsToken(triviaParser, VJassSyntaxKind.CloseBracketToken, VJassSymbol.CloseBracket));
        }
    }
}