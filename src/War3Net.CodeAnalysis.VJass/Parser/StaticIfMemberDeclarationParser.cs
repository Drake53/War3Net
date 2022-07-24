// ------------------------------------------------------------------------------
// <copyright file="StaticIfMemberDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassMemberDeclarationSyntax> GetStaticIfMemberDeclarationParser(
            Parser<char, VJassMemberDeclarationSyntax> memberDeclarationParser,
            Parser<char, VJassStaticIfClauseDeclaratorSyntax> staticIfClauseDeclaratorParser,
            Parser<char, VJassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return memberDeclarationParser.IfThenElse(
                leadingTriviaParser,
                staticIfClauseDeclaratorParser,
                elseIfClauseDeclaratorParser,
                Keyword.Else.AsToken(trailingTriviaParser, VJassSyntaxKind.ElseKeyword),
                Keyword.EndIf.AsToken(trailingTriviaParser, VJassSyntaxKind.EndIfKeyword),
                (declarator, declarations) => new VJassMemberDeclarationStaticIfClauseSyntax(declarator, declarations.ToImmutableArray()),
                (declarator, declarations) => new VJassMemberDeclarationElseIfClauseSyntax(declarator, declarations.ToImmutableArray()),
                (elseToken, declarations) => new VJassMemberDeclarationElseClauseSyntax(elseToken, declarations.ToImmutableArray()),
                (trivia, declaration) => declaration.WithLeadingTrivia(trivia),
                (trivia, elseIfDeclarator) => elseIfDeclarator.WithLeadingTrivia(trivia),
                (trivia, elseDeclarator) => elseDeclarator.WithLeadingTrivia(trivia),
                (staticIfClause, elseIfClauses, elseClause, trivia, endIfToken) => (VJassMemberDeclarationSyntax)new VJassStaticIfMemberDeclarationSyntax(
                    staticIfClause,
                    elseIfClauses.ToImmutableArray(),
                    elseClause,
                    endIfToken.WithLeadingTrivia(trivia)));
        }
    }
}