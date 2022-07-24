// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassFunctionDeclarationSyntax> GetFunctionDeclarationParser(
            Parser<char, VJassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, VJassStatementSyntax> statementParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading(
                leadingTriviaParser,
                functionDeclaratorParser,
                Keyword.EndFunction.AsToken(trailingTriviaParser, VJassSyntaxKind.EndFunctionKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (declarator, statements, leadingTrivia, endFunctionToken) => new VJassFunctionDeclarationSyntax(
                    ImmutableArray<VJassModifierSyntax>.Empty,
                    declarator,
                    statements.ToImmutableArray(),
                    endFunctionToken.WithLeadingTrivia(leadingTrivia)));
        }

        internal static Parser<char, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>> GetScopedFunctionDeclarationParser(
            Parser<char, VJassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, VJassStatementSyntax> statementParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading<char, VJassSyntaxTriviaList, VJassFunctionDeclaratorSyntax, VJassStatementSyntax, VJassSyntaxToken, Func<IEnumerable<VJassModifierSyntax>, VJassScopedDeclarationSyntax>>(
                leadingTriviaParser,
                functionDeclaratorParser,
                Keyword.EndFunction.AsToken(trailingTriviaParser, VJassSyntaxKind.EndFunctionKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (declarator, statements, leadingTrivia, endFunctionToken) => modifiers => new VJassFunctionDeclarationSyntax(
                    modifiers.ToImmutableArray(),
                    declarator,
                    statements.ToImmutableArray(),
                    endFunctionToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}