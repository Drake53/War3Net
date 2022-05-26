// ------------------------------------------------------------------------------
// <copyright file="LoopStatementParser.cs" company="Drake53">
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
        internal static Parser<char, IStatementSyntax> GetLoopStatementParser(
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, Unit> whitespaceParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Loop.Then(whitespaceParser).Then(endOfLineParser)
                .Then(statementListParser).Before(Keyword.EndLoop.Then(whitespaceParser))
                .Select<IStatementSyntax>(statementList => new JassLoopStatementSyntax(statementList));
        }
    }
}