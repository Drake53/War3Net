// ------------------------------------------------------------------------------
// <copyright file="LibraryDependencyListParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassLibraryDependencyListSyntax> GetLibraryDependencyListParser(
            Parser<char, VJassLibraryDependencySyntax> libraryDependencyParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (requiresToken, dependencies) => new VJassLibraryDependencyListSyntax(
                    requiresToken,
                    dependencies.ToImmutableArray()),
                OneOf(
                    Keyword.Requires.AsToken(triviaParser, VJassSyntaxKind.RequiresKeyword),
                    Keyword.Needs.AsToken(triviaParser, VJassSyntaxKind.NeedsKeyword),
                    Keyword.Uses.AsToken(triviaParser, VJassSyntaxKind.UsesKeyword)),
                libraryDependencyParser.AtLeastOnce());
        }
    }
}