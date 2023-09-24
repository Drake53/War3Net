// ------------------------------------------------------------------------------
// <copyright file="IfStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassStatementSyntax> GetIfStatementParser(
            Parser<char, JassStatementSyntax> statementParser,
            Parser<char, JassIfClauseDeclaratorSyntax> ifClauseDeclaratorParser,
            Parser<char, JassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.IfThenElse(
                leadingTriviaParser,
                ifClauseDeclaratorParser,
                elseIfClauseDeclaratorParser,
                Keyword.Else.AsToken(trailingTriviaParser, JassSyntaxKind.ElseKeyword),
                Keyword.EndIf.AsToken(trailingTriviaParser, JassSyntaxKind.EndIfKeyword),
                (declarator, statements) => new JassIfClauseSyntax(declarator, statements.ToImmutableArray()),
                (declarator, statements) => new JassElseIfClauseSyntax(declarator, statements.ToImmutableArray()),
                (elseToken, statements) => new JassElseClauseSyntax(elseToken, statements.ToImmutableArray()),
                (trivia, statement) => statement.WithLeadingTrivia(trivia),
                (trivia, elseIfDeclarator) => elseIfDeclarator.WithLeadingTrivia(trivia),
                (trivia, elseDeclarator) => elseDeclarator.WithLeadingTrivia(trivia),
                (ifClause, elseIfClauses, elseClause, trivia, endIfToken) => (JassStatementSyntax)new JassIfStatementSyntax(
                    ifClause,
                    elseIfClauses.ToImmutableArray(),
                    elseClause,
                    endIfToken.WithLeadingTrivia(trivia)));
        }
    }
}