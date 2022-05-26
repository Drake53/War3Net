// ------------------------------------------------------------------------------
// <copyright file="CallStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassCallStatementSyntax> GetCallStatementParser(
            Parser<char, Unit> whitespaceParser,
            Parser<char, JassArgumentListSyntax> argumentListParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return Keyword.Call.Then(whitespaceParser).Then(identifierNameParser).Then(
                Symbol.LeftParenthesis.Before(whitespaceParser).Then(argumentListParser).Before(Symbol.RightParenthesis.Before(whitespaceParser)),
                (id, arguments) => new JassCallStatementSyntax(id, arguments));
        }
    }
}