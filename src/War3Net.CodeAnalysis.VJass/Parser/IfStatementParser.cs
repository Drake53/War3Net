// ------------------------------------------------------------------------------
// <copyright file="IfStatementParser.cs" company="Drake53">
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
        internal static Parser<char, VJassStatementSyntax> GetIfStatementParser(
            Parser<char, VJassStatementSyntax> statementParser,
            Parser<char, VJassIfClauseDeclaratorSyntax> ifClauseDeclaratorParser,
            Parser<char, VJassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.IfThenElse(
                leadingTriviaParser,
                ifClauseDeclaratorParser,
                elseIfClauseDeclaratorParser,
                Keyword.Else.AsToken(trailingTriviaParser, VJassSyntaxKind.ElseKeyword),
                Keyword.EndIf.AsToken(trailingTriviaParser, VJassSyntaxKind.EndIfKeyword),
                (declarator, statements) => new VJassStatementIfClauseSyntax(declarator, statements.ToImmutableArray()),
                (declarator, statements) => new VJassStatementElseIfClauseSyntax(declarator, statements.ToImmutableArray()),
                (elseToken, statements) => new VJassStatementElseClauseSyntax(elseToken, statements.ToImmutableArray()),
                (trivia, statement) => statement.WithLeadingTrivia(trivia),
                (trivia, elseIfDeclarator) => elseIfDeclarator.WithLeadingTrivia(trivia),
                (trivia, elseDeclarator) => elseDeclarator.WithLeadingTrivia(trivia),
                (ifClause, elseIfClauses, elseClause, trivia, endIfToken) => (VJassStatementSyntax)new VJassIfStatementSyntax(
                    ifClause,
                    elseIfClauses.ToImmutableArray(),
                    elseClause,
                    endIfToken.WithLeadingTrivia(trivia)));
        }
    }
}