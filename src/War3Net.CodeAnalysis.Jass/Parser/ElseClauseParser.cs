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
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, Unit> whitespaceParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Else.Then(whitespaceParser).Before(endOfLineParser)
                .Then(statementListParser)
                .Select(statementList => new JassElseClauseSyntax(statementList));
        }
    }
}