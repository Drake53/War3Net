// ------------------------------------------------------------------------------
// <copyright file="ElseClauseParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassElseClauseSyntax> GetElseClauseParser(
            Parser<char, IStatementSyntax> statementParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Else.Before(endOfLineParser)
                .Then(GetStatementListParser(statementParser, endOfLineParser))
                .Select(statementList => new JassElseClauseSyntax(statementList));
        }
    }
}