// ------------------------------------------------------------------------------
// <copyright file="EmptyParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassEmptySyntax> GetEmptyParser()
        {
            return Lookahead(Symbol.CarriageReturn.Or(Symbol.LineFeed)).ThenReturn(JassEmptySyntax.Value);
        }

        internal static Parser<char, JassEmptySyntax> GetEmptyLineParser()
        {
            return Lookahead(End).ThenReturn(JassEmptySyntax.Value);
        }
    }
}