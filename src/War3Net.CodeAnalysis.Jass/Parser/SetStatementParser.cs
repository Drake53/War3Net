// ------------------------------------------------------------------------------
// <copyright file="SetStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassSetStatementSyntax> GetSetStatementParser(
            Parser<char, Unit> whitespaceParser,
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return Map(
                (id, indexer, equals) => new JassSetStatementSyntax(id, indexer.GetValueOrDefault(), equals),
                Keyword.Set.Then(identifierNameParser),
                Symbol.LeftSquareBracket.Before(whitespaceParser).Then(expressionParser).Before(Symbol.RightSquareBracket.Before(whitespaceParser)).Optional(),
                equalsValueClauseParser);
        }
    }
}