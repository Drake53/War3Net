// ------------------------------------------------------------------------------
// <copyright file="ParameterListParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassParameterListOrEmptyParameterListSyntax> GetParameterListParser(
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassParameterSyntax> parameterParser)
        {
            return Map(
                (takesToken, parameterListFunc) => parameterListFunc(takesToken),
                Keyword.Takes.AsToken(triviaParser, JassSyntaxKind.TakesKeyword),
                OneOf(
                    Map<char, JassSyntaxToken, Func<JassSyntaxToken, JassParameterListOrEmptyParameterListSyntax>>(
                        (nothingToken) => takesToken => new JassEmptyParameterListSyntax(
                            takesToken,
                            nothingToken),
                        Keyword.Nothing.AsToken(triviaParser, JassSyntaxKind.NothingKeyword)),
                    Map<char, SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken>, Func<JassSyntaxToken, JassParameterListOrEmptyParameterListSyntax>>(
                        (parameterList) => takesToken => new JassParameterListSyntax(
                            takesToken,
                            parameterList),
                        parameterParser.SeparatedList(Symbol.Comma.AsToken(triviaParser, JassSyntaxKind.CommaToken, JassSymbol.Comma)))));
        }
    }
}