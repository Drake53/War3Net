// ------------------------------------------------------------------------------
// <copyright file="DebugStatementParser.cs" company="Drake53">
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
        internal static Parser<char, IStatementSyntax> GetDebugStatementParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, IStatementSyntax> setStatementParser,
            Parser<char, IStatementSyntax> callStatementParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Debug.Then(
                OneOf(
                    setStatementParser,
                    callStatementParser,
                    GetIfStatementParser(expressionParser, statementListParser, endOfLineParser),
                    GetLoopStatementParser(statementListParser, endOfLineParser))
                .Select<IStatementSyntax>(statement => new JassDebugStatementSyntax(statement)));
        }
    }
}