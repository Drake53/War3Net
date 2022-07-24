// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassTopLevelDeclarationSyntax> GetTopLevelDeclarationParser(
            Parser<char, VJassTypeDeclarationSyntax> typeDeclarationParser,
            Parser<char, VJassNativeFunctionDeclarationSyntax> nativeFunctionDeclarationParser,
            Parser<char, VJassFunctionDeclarationSyntax> functionDeclarationParser,
            Parser<char, VJassLibraryDeclarationSyntax> libraryDeclarationParser,
            Parser<char, VJassScopeDeclarationSyntax> scopeDeclarationParser,
            Parser<char, VJassGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return OneOf(
                typeDeclarationParser.Cast<VJassTopLevelDeclarationSyntax>(),
                GetGlobalsDeclarationParser(globalDeclarationParser, triviaParser, leadingTriviaParser, trailingTriviaParser).Cast<VJassTopLevelDeclarationSyntax>(),
                nativeFunctionDeclarationParser.Cast<VJassTopLevelDeclarationSyntax>(),
                functionDeclarationParser.Cast<VJassTopLevelDeclarationSyntax>(),
                libraryDeclarationParser.Cast<VJassTopLevelDeclarationSyntax>(),
                scopeDeclarationParser.Cast<VJassTopLevelDeclarationSyntax>());
        }
    }
}