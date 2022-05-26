// ------------------------------------------------------------------------------
// <copyright file="ExitStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassExitStatementSyntax> GetExitStatementParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Keyword.ExitWhen.Then(whitespaceParser).Then(expressionParser)
                .Select(expression => new JassExitStatementSyntax(expression));
        }
    }
}