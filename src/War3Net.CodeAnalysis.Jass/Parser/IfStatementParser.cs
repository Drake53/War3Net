// ------------------------------------------------------------------------------
// <copyright file="IfStatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IStatementSyntax> GetIfStatementParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, IStatementSyntax> statementParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Map(
                (condition, statementList, elseIfClauses, elseClause, _) => (IStatementSyntax)new JassIfStatementSyntax(condition, statementList, elseIfClauses.ToImmutableArray(), elseClause.GetValueOrDefault()),
                Keyword.If.Then(expressionParser).Before(Keyword.Then).Before(endOfLineParser),
                GetStatementListParser(statementParser, endOfLineParser),
                GetElseIfClauseParser(expressionParser, statementParser, endOfLineParser).Many(),
                GetElseClauseParser(statementParser, endOfLineParser).Optional(),
                Keyword.EndIf);
        }
    }
}