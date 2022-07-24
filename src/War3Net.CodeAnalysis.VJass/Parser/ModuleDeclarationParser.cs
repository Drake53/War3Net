// ------------------------------------------------------------------------------
// <copyright file="ModuleDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassModuleDeclarationSyntax> GetModuleDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassModuleDeclaratorSyntax> moduleDeclaratorParser,
            Parser<char, VJassMemberDeclarationSyntax> memberDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return memberDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                moduleDeclaratorParser,
                Keyword.EndModule.AsToken(trailingTriviaParser, VJassSyntaxKind.EndModuleKeyword),
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (declarator, declarations, leadingTrivia, endModuleToken) => new VJassModuleDeclarationSyntax(
                    ImmutableArray<VJassModifierSyntax>.Empty,
                    declarator,
                    declarations.ToImmutableArray(),
                    endModuleToken.WithLeadingTrivia(leadingTrivia)));
        }

        internal static Parser<char, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>> GetScopedModuleDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassModuleDeclaratorSyntax> moduleDeclaratorParser,
            Parser<char, VJassMemberDeclarationSyntax> memberDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return memberDeclarationParser.UntilWithLeading<char, VJassSyntaxTriviaList, VJassModuleDeclaratorSyntax, VJassMemberDeclarationSyntax, VJassSyntaxToken, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>>(
                leadingTriviaParser,
                moduleDeclaratorParser,
                Keyword.EndModule.AsToken(trailingTriviaParser, VJassSyntaxKind.EndModuleKeyword),
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (declarator, declarations, leadingTrivia, endModuleToken) => modifiers => new VJassModuleDeclarationSyntax(
                    modifiers.ToImmutableArray(),
                    declarator,
                    declarations.ToImmutableArray(),
                    endModuleToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}