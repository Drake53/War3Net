// ------------------------------------------------------------------------------
// <copyright file="ElseClauseFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static ElseClauseSyntax ElseClause(ElseSyntax @else)
        {
            return new ElseClauseSyntax(@else);
        }

        public static ElseClauseSyntax ElseClause(params NewStatementSyntax[] statements)
        {
            return new ElseClauseSyntax(new ElseSyntax(Token(SyntaxTokenType.ElseKeyword), Newlines(), new StatementListSyntax(statements)));
        }

        public static ElseClauseSyntax ElseClause(NewExpressionSyntax condition, params NewStatementSyntax[] statements)
        {
            return new ElseClauseSyntax(
                new ElseifSyntax(
                    Token(SyntaxTokenType.ElseifKeyword),
                    condition,
                    Token(SyntaxTokenType.ThenKeyword),
                    Newlines(),
                    new StatementListSyntax(statements),
                    Empty()));
        }

        public static ElseClauseSyntax ElseClause(ElseClauseSyntax elseClause, NewExpressionSyntax condition, params NewStatementSyntax[] statements)
        {
            return new ElseClauseSyntax(
                new ElseifSyntax(
                    Token(SyntaxTokenType.ElseifKeyword),
                    condition,
                    Token(SyntaxTokenType.ThenKeyword),
                    Newlines(),
                    new StatementListSyntax(statements),
                    elseClause));
        }
    }
}