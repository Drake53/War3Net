// ------------------------------------------------------------------------------
// <copyright file="EmptyStatementParser.cs" company="Drake53">
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
        internal static Parser<char, JassEmptyStatementSyntax> GetEmptyStatementParser()
        {
            return Lookahead(Symbol.CarriageReturn.Or(Symbol.LineFeed)).ThenReturn(JassEmptyStatementSyntax.Value);
        }
    }
}