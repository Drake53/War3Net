// ------------------------------------------------------------------------------
// <copyright file="StatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassStatementSyntax> GetStatementParser(
            Parser<char, VJassLocalVariableDeclarationStatementSyntax> localVariableDeclarationStatementParser,
            Parser<char, VJassExitStatementSyntax> exitStatementParser,
            Parser<char, VJassReturnStatementSyntax> returnStatementParser,
            Parser<char, VJassSetStatementSyntax> setStatementParser,
            Parser<char, VJassCallStatementSyntax> callStatementParser,
            Parser<char, VJassIfClauseDeclaratorSyntax> ifClauseDeclaratorParser,
            Parser<char, VJassStaticIfClauseDeclaratorSyntax> staticIfClauseDeclaratorParser,
            Parser<char, VJassElseIfClauseDeclaratorSyntax> elseIfClauseDeclaratorParser,
            Parser<char, VJassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Rec<char, VJassStatementSyntax>(
                statementParser =>
                {
                    return OneOf(
                        localVariableDeclarationStatementParser.Cast<VJassStatementSyntax>(),
                        setStatementParser.Cast<VJassStatementSyntax>(),
                        callStatementParser.Cast<VJassStatementSyntax>(),
                        GetIfStatementParser(statementParser, ifClauseDeclaratorParser, elseIfClauseDeclaratorParser, leadingTriviaParser, trailingTriviaParser),
                        GetLoopStatementParser(statementParser, leadingTriviaParser, trailingTriviaParser),
                        exitStatementParser.Cast<VJassStatementSyntax>(),
                        returnStatementParser.Cast<VJassStatementSyntax>(),
                        GetStaticIfStatementParser(statementParser, staticIfClauseDeclaratorParser, elseIfClauseDeclaratorParser, leadingTriviaParser, trailingTriviaParser));
                });
        }
    }
}