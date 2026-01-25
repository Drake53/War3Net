// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassCompilationUnitSyntax> GetCompilationUnitParser(
            Parser<char, JassTopLevelDeclarationSyntax> declarationParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser)
        {
            return declarationParser.UntilWithLeading(
                leadingTriviaParser,
                leadingTriviaParser,
                End,
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (firstTrivia, declarations, lastTrivia, _) => new JassCompilationUnitSyntax(
                    declarations.ToImmutableArray(),
                    new JassSyntaxToken(lastTrivia, JassSyntaxKind.EndOfFileToken, string.Empty, JassSyntaxTriviaList.Empty)).WithLeadingTrivia(firstTrivia));
        }
    }
}