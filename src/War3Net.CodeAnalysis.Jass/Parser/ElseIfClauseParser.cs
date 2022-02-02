// ------------------------------------------------------------------------------
// <copyright file="ElseIfClauseParser.cs" company="Drake53">
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
        internal static Parser<char, JassElseIfClauseSyntax> GetElseIfClauseParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Map(
                (condition, statementList) => new JassElseIfClauseSyntax(condition, statementList),
                Keyword.ElseIf.Then(expressionParser).Before(Keyword.Then).Before(endOfLineParser),
                statementListParser);
        }
    }
}