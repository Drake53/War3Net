// ------------------------------------------------------------------------------
// <copyright file="ScopedGlobalsDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassScopedGlobalsDeclarationSyntax> GetScopedGlobalsDeclarationParser(
            Parser<char, VJassScopedGlobalDeclarationSyntax> scopedGlobalDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return scopedGlobalDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                Keyword.Globals.AsToken(trailingTriviaParser, VJassSyntaxKind.GlobalsKeyword),
                Keyword.EndGlobals.AsToken(trailingTriviaParser, VJassSyntaxKind.EndGlobalsKeyword),
                (leadingTrivia, global) => global.WithLeadingTrivia(leadingTrivia),
                (globalsToken, globals, leadingTrivia, endGlobalsToken) => new VJassScopedGlobalsDeclarationSyntax(
                    globalsToken,
                    globals.ToImmutableArray(),
                    endGlobalsToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}