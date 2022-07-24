// ------------------------------------------------------------------------------
// <copyright file="StructDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassStructDeclarationSyntax> GetStructDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassStructDeclaratorSyntax> structDeclaratorParser,
            Parser<char, VJassMemberDeclarationSyntax> memberDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return memberDeclarationParser.UntilWithLeading(
                leadingTriviaParser,
                structDeclaratorParser,
                Keyword.EndStruct.AsToken(trailingTriviaParser, VJassSyntaxKind.EndStructKeyword),
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (declarator, declarations, leadingTrivia, endStructToken) => new VJassStructDeclarationSyntax(
                    ImmutableArray<VJassModifierSyntax>.Empty,
                    declarator,
                    declarations.ToImmutableArray(),
                    endStructToken.WithLeadingTrivia(leadingTrivia)));
        }

        internal static Parser<char, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>> GetScopedStructDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassStructDeclaratorSyntax> structDeclaratorParser,
            Parser<char, VJassMemberDeclarationSyntax> memberDeclarationParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return memberDeclarationParser.UntilWithLeading<char, VJassSyntaxTriviaList, VJassStructDeclaratorSyntax, VJassMemberDeclarationSyntax, VJassSyntaxToken, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>>(
                leadingTriviaParser,
                structDeclaratorParser,
                Keyword.EndStruct.AsToken(trailingTriviaParser, VJassSyntaxKind.EndStructKeyword),
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (declarator, declarations, leadingTrivia, endStructToken) => modifiers => new VJassStructDeclarationSyntax(
                    modifiers.ToImmutableArray(),
                    declarator,
                    declarations.ToImmutableArray(),
                    endStructToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}