// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassGlobalsDeclarationSyntax> GetGlobalsDeclarationParser(
            Parser<char, VJassGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return globalDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                Keyword.Globals.AsToken(trailingTriviaParser, VJassSyntaxKind.GlobalsKeyword),
                Keyword.EndGlobals.AsToken(trailingTriviaParser, VJassSyntaxKind.EndGlobalsKeyword),
                (leadingTrivia, global) => global.WithLeadingTrivia(leadingTrivia),
                (globalsToken, globals, leadingTrivia, endGlobalsToken) => new VJassGlobalsDeclarationSyntax(
                    globalsToken,
                    globals.ToImmutableArray(),
                    endGlobalsToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}