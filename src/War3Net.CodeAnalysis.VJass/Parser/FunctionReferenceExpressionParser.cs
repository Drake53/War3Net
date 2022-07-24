// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, VJassExpressionSyntax> GetFunctionReferenceExpressionParser(
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (functionToken, expression) => (VJassExpressionSyntax)new VJassFunctionReferenceExpressionSyntax(
                    functionToken,
                    expression),
                Keyword.Function.AsToken(triviaParser, VJassSyntaxKind.FunctionKeyword),
                expressionParser)
                .Labelled("function reference");
        }
    }
}