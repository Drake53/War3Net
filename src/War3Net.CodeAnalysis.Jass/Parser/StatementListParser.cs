// ------------------------------------------------------------------------------
// <copyright file="StatementListParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassStatementListSyntax> GetStatementListParser(
            Parser<char, IStatementSyntax> statementParser,
            Parser<char, Unit> endOfLineParser)
        {
            return statementParser.Before(endOfLineParser).Many()
                .Select(statements => new JassStatementListSyntax(statements.ToImmutableArray()));
        }
    }
}