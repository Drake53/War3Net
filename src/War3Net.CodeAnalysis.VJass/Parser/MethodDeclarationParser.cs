// ------------------------------------------------------------------------------
// <copyright file="MethodDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, Func<IEnumerable<VJassModifierSyntax>, VJassMemberDeclarationSyntax>> GetMethodDeclarationParser(
            Parser<char, VJassMethodOrOperatorDeclaratorSyntax> methodDeclaratorParser,
            Parser<char, VJassStatementSyntax> statementParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading<char, VJassSyntaxTriviaList, VJassMethodOrOperatorDeclaratorSyntax, VJassStatementSyntax, VJassSyntaxToken, Func<IEnumerable<VJassModifierSyntax>, VJassMemberDeclarationSyntax>>(
                leadingTriviaParser,
                methodDeclaratorParser,
                Keyword.EndMethod.AsToken(trailingTriviaParser, VJassSyntaxKind.EndMethodKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (declarator, statements, leadingTrivia, endMethodToken) => modifiers => new VJassMethodDeclarationSyntax(
                    modifiers.ToImmutableArray(),
                    declarator,
                    statements.ToImmutableArray(),
                    endMethodToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}