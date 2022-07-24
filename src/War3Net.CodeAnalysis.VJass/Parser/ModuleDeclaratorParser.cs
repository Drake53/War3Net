// ------------------------------------------------------------------------------
// <copyright file="ModuleDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, VJassModuleDeclaratorSyntax> GetModuleDeclaratorParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassInitializerSyntax?> initializerParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (moduleToken, identifierName, initializer, trailingTrivia) => new VJassModuleDeclaratorSyntax(
                    moduleToken,
                    identifierName,
                    initializer.GetValueOrDefault()).AppendTrivia(trailingTrivia),
                Keyword.Module.AsToken(triviaParser, VJassSyntaxKind.ModuleKeyword),
                identifierNameParser.Labelled("module identifier name"),
                initializerParser.Optional(),
                trailingTriviaParser);
        }
    }
}