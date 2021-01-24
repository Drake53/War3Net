// ------------------------------------------------------------------------------
// <copyright file="CommentParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, string> GetCommentParser()
        {
            return Try(String($"{JassSymbol.Slash}{JassSymbol.Slash}")).Then(AnyCharExcept(JassSymbol.CarriageReturn, JassSymbol.LineFeed).ManyString());
        }
    }
}