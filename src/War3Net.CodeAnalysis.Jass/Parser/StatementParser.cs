// ------------------------------------------------------------------------------
// <copyright file="StatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;
using Pidgin.Expression;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IStatementSyntax> GetStatementParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, IStatementSyntax> setStatementParser,
            Parser<char, IStatementSyntax> callStatementParser,
            Parser<char, IVariableDeclarator> variableDeclaratorParser,
            Parser<char, string> commentParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Pidgin.Expression.ExpressionParser.Build<char, IStatementSyntax>(
                statementParser =>
                (
                    OneOf(
                        GetEmptyStatementParser().Cast<IStatementSyntax>(),
                        GetCommentStatementParser(commentParser).Cast<IStatementSyntax>(),
                        GetLocalVariableDeclarationStatementParser(variableDeclaratorParser).Cast<IStatementSyntax>(),
                        setStatementParser,
                        callStatementParser,
                        GetIfStatementParser(expressionParser, statementParser, endOfLineParser),
                        GetLoopStatementParser(statementParser, endOfLineParser),
                        GetExitStatementParser(expressionParser).Cast<IStatementSyntax>(),
                        GetReturnStatementParser(expressionParser).Cast<IStatementSyntax>(),
                        GetDebugStatementParser(expressionParser, statementParser, setStatementParser, callStatementParser, endOfLineParser)),
                    Array.Empty<OperatorTableRow<char, IStatementSyntax>>()));
        }
    }
}