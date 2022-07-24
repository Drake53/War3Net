// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassCompilationUnitSyntax> GetCompilationUnitParser(
            Parser<char, VJassTopLevelDeclarationSyntax> declarationParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser)
        {
            return declarationParser.UntilWithLeading(
                leadingTriviaParser,
                leadingTriviaParser,
                End,
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (firstTrivia, declarations, lastTrivia, _) => new VJassCompilationUnitSyntax(
                    declarations.ToImmutableArray(),
                    new VJassSyntaxToken(lastTrivia, VJassSyntaxKind.EndOfFileToken, string.Empty, VJassSyntaxTriviaList.Empty)).WithLeadingTrivia(firstTrivia));
        }
    }
}