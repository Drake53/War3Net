// ------------------------------------------------------------------------------
// <copyright file="LibraryDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassLibraryDeclarationSyntax> GetLibraryDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassLibraryDeclaratorSyntax> libraryDeclaratorParser,
            Parser<char, VJassScopedDeclarationSyntax> scopedDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return scopedDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                libraryDeclaratorParser,
                Keyword.EndLibrary.AsToken(trailingTriviaParser, VJassSyntaxKind.EndLibraryKeyword),
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (declarator, declarations, leadingTrivia, endLibraryToken) => new VJassLibraryDeclarationSyntax(
                    declarator,
                    declarations.ToImmutableArray(),
                    endLibraryToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}