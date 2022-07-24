// ------------------------------------------------------------------------------
// <copyright file="StaticIfScopedDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassScopedDeclarationSyntax> GetStaticIfScopedDeclarationParser(
            Parser<char, VJassScopedDeclarationSyntax> scopedDeclarationParser,
            Parser<char, VJassStaticIfClauseDeclaratorSyntax> staticIfClauseDeclaratorParser,
            Parser<char, VJassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return scopedDeclarationParser.IfThenElse(
                leadingTriviaParser,
                staticIfClauseDeclaratorParser,
                elseIfClauseDeclaratorParser,
                Keyword.Else.AsToken(trailingTriviaParser, VJassSyntaxKind.ElseKeyword),
                Keyword.EndIf.AsToken(trailingTriviaParser, VJassSyntaxKind.EndIfKeyword),
                (declarator, declarations) => new VJassScopedDeclarationStaticIfClauseSyntax(declarator, declarations.ToImmutableArray()),
                (declarator, declarations) => new VJassScopedDeclarationElseIfClauseSyntax(declarator, declarations.ToImmutableArray()),
                (elseToken, declarations) => new VJassScopedDeclarationElseClauseSyntax(elseToken, declarations.ToImmutableArray()),
                (trivia, declaration) => declaration.WithLeadingTrivia(trivia),
                (trivia, elseIfDeclarator) => elseIfDeclarator.WithLeadingTrivia(trivia),
                (trivia, elseDeclarator) => elseDeclarator.WithLeadingTrivia(trivia),
                (staticIfClause, elseIfClauses, elseClause, trivia, endIfToken) => (VJassScopedDeclarationSyntax)new VJassStaticIfScopedDeclarationSyntax(
                    staticIfClause,
                    elseIfClauses.ToImmutableArray(),
                    elseClause,
                    endIfToken.WithLeadingTrivia(trivia)));
        }
    }
}