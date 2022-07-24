// ------------------------------------------------------------------------------
// <copyright file="ParameterListParser.cs" company="Drake53">
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
        internal static Parser<char, VJassParameterListOrEmptyParameterListSyntax> GetParameterListParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassParameterSyntax> parameterParser)
        {
            return Map(
                (takesToken, parameterListFunc) => parameterListFunc(takesToken),
                Keyword.Takes.AsToken(triviaParser, VJassSyntaxKind.TakesKeyword),
                OneOf(
                    Map<char, VJassSyntaxToken, Func<VJassSyntaxToken, VJassParameterListOrEmptyParameterListSyntax>>(
                        nothingToken => takesToken => new VJassEmptyParameterListSyntax(
                            takesToken,
                            nothingToken),
                        Keyword.Nothing.AsToken(triviaParser, VJassSyntaxKind.NothingKeyword)),
                    Map<char, SeparatedSyntaxList<VJassParameterSyntax, VJassSyntaxToken>, Func<VJassSyntaxToken, VJassParameterListOrEmptyParameterListSyntax>>(
                        (parameterList) => takesToken => new VJassParameterListSyntax(
                            takesToken,
                            parameterList),
                        parameterParser.SeparatedList(Symbol.Comma.AsToken(triviaParser, VJassSyntaxKind.CommaToken, VJassSymbol.Comma)))));
        }
    }
}