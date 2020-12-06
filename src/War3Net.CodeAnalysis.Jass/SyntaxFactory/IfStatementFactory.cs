// ------------------------------------------------------------------------------
// <copyright file="IfStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewStatementSyntax IfStatement(NewExpressionSyntax condition, params NewStatementSyntax[] ifBody)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new IfStatementSyntax(
                        Token(SyntaxTokenType.IfKeyword),
                        condition,
                        Token(SyntaxTokenType.ThenKeyword),
                        Newlines(),
                        StatementList(ifBody),
                        Empty(),
                        Token(SyntaxTokenType.EndifKeyword))),
                Newlines(2));
        }

        public static NewStatementSyntax IfStatement(NewExpressionSyntax condition, ElseClauseSyntax elseClause, params NewStatementSyntax[] ifBody)
        {
            return new NewStatementSyntax(
                new StatementSyntax(
                    new IfStatementSyntax(
                        Token(SyntaxTokenType.IfKeyword),
                        condition,
                        Token(SyntaxTokenType.ThenKeyword),
                        Newlines(),
                        StatementList(ifBody),
                        elseClause,
                        Token(SyntaxTokenType.EndifKeyword))),
                Newlines(2));
        }

        public static NewStatementSyntax IfStatement(params (NewExpressionSyntax condition, NewStatementSyntax[] body)[] ifElseifBlocks)
        {
            return IfStatement(null, ifElseifBlocks);
        }

        public static NewStatementSyntax IfStatement(ElseClauseSyntax? elseClause, params (NewExpressionSyntax condition, NewStatementSyntax[] body)[] ifElseifBlocks)
        {
            if (ifElseifBlocks.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(ifElseifBlocks));
            }

            var previousClause = elseClause;
            var enumerable = ifElseifBlocks.Skip(1).Reverse();

            // No else block, only elseif.
            if (previousClause is null)
            {
                var (condition, body) = ifElseifBlocks.Last();
                previousClause = ElseClause(condition, body);
                enumerable = enumerable.Skip(1);
            }

            foreach (var (condition, body) in enumerable)
            {
                previousClause = ElseClause(previousClause, condition, body);
            }

            var ifBlock = ifElseifBlocks.First();
            return IfStatement(ifBlock.condition, previousClause, ifBlock.body);
        }
    }
}