// ------------------------------------------------------------------------------
// <copyright file="StatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementSyntax> GetStatementParser(
            Parser<char, JassStatementSyntax> localVariableDeclarationStatementParser,
            Parser<char, JassStatementSyntax> exitStatementParser,
            Parser<char, JassStatementSyntax> returnStatementParser,
            Parser<char, JassStatementSyntax> setStatementParser,
            Parser<char, JassStatementSyntax> callStatementParser,
            Parser<char, JassIfClauseDeclaratorSyntax> ifClauseDeclaratorParser,
            Parser<char, JassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Rec<char, JassStatementSyntax>(
                statementParser =>
                {
                    var ifStatementParser = GetIfStatementParser(statementParser, ifClauseDeclaratorParser, elseIfClauseDeclaratorParser, leadingTriviaParser, trailingTriviaParser);
                    var loopStatementParser = GetLoopStatementParser(statementParser, leadingTriviaParser, trailingTriviaParser);

                    return OneOf(
                        localVariableDeclarationStatementParser,
                        setStatementParser,
                        callStatementParser,
                        ifStatementParser,
                        loopStatementParser,
                        exitStatementParser,
                        returnStatementParser,
                        GetDebugStatementParser(
                            setStatementParser,
                            callStatementParser,
                            ifStatementParser,
                            loopStatementParser,
                            triviaParser,
                            trailingTriviaParser));
                });
        }
    }
}