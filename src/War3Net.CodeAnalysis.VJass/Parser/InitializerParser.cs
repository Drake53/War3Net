﻿// ------------------------------------------------------------------------------
// <copyright file="InitializerParser.cs" company="Drake53">
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
        internal static Parser<char, VJassInitializerSyntax> GetInitializerParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (initializerToken, identifierName) => new VJassInitializerSyntax(
                    initializerToken,
                    identifierName),
                Keyword.Initializer.AsToken(triviaParser, VJassSyntaxKind.InitializerKeyword),
                identifierNameParser.Labelled("initializer identifier name"));
        }
    }
}