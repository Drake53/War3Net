// ------------------------------------------------------------------------------
// <copyright file="ModuleImplementationDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassModuleImplementationDeclarationSyntax> GetModuleImplementationDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (implementToken, optionalToken, identifierName) => new VJassModuleImplementationDeclarationSyntax(
                    implementToken,
                    optionalToken.GetValueOrDefault(),
                    identifierName),
                Keyword.Implement.AsToken(triviaParser, VJassSyntaxKind.ImplementKeyword),
                Keyword.Optional.AsToken(triviaParser, VJassSyntaxKind.OptionalKeyword).Optional(),
                identifierNameParser);
        }
    }
}