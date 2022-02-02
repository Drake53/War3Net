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
            Parser<char, JassEmptySyntax> emptyParser,
            Parser<char, JassCommentSyntax> commentParser,
            Parser<char, JassLocalVariableDeclarationStatementSyntax> localVariableDeclarationStatementParser,
            Parser<char, JassExitStatementSyntax> exitStatementParser,
            Parser<char, JassReturnStatementSyntax> returnStatementParser,
            Parser<char, JassSetStatementSyntax> setStatementParser,
            Parser<char, JassCallStatementSyntax> callStatementParser,
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, Unit> endOfLineParser)
        {
            var setParser = setStatementParser.Cast<IStatementSyntax>();
            var callParser = callStatementParser.Cast<IStatementSyntax>();

            return Pidgin.Expression.ExpressionParser.Build<char, IStatementSyntax>(
                statementParser =>
                {
                    var statementListParser = GetStatementListParser(
                        statementParser,
                        endOfLineParser);

                    return (OneOf(
                        emptyParser.Cast<IStatementSyntax>(),
                        commentParser.Cast<IStatementSyntax>(),
                        localVariableDeclarationStatementParser.Cast<IStatementSyntax>(),
                        setParser,
                        callParser,
                        GetIfStatementParser(expressionParser, statementListParser, endOfLineParser),
                        GetLoopStatementParser(statementListParser, endOfLineParser),
                        exitStatementParser.Cast<IStatementSyntax>(),
                        returnStatementParser.Cast<IStatementSyntax>(),
                        GetDebugStatementParser(expressionParser, statementListParser, setParser, callParser, endOfLineParser)),
                    Array.Empty<OperatorTableRow<char, IStatementSyntax>>());
                });
        }
    }
}