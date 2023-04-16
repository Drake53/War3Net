// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTopLevelDeclarationSyntax> GetGlobalsDeclarationParser(
            Parser<char, JassGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return globalDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                Keyword.Globals.AsToken(trailingTriviaParser, JassSyntaxKind.GlobalsKeyword),
                Keyword.EndGlobals.AsToken(trailingTriviaParser, JassSyntaxKind.EndGlobalsKeyword),
                (leadingTrivia, global) => global.WithLeadingTrivia(leadingTrivia),
                (globalsToken, globalDeclarations, leadingTrivia, endGlobalsToken) => (JassTopLevelDeclarationSyntax)new JassGlobalsDeclarationSyntax(
                    globalsToken,
                    globalDeclarations.ToImmutableArray(),
                    endGlobalsToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}