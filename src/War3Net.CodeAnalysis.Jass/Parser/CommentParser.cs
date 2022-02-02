// ------------------------------------------------------------------------------
// <copyright file="CommentParser.cs" company="Drake53">
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
        internal static Parser<char, string> GetCommentStringParser()
        {
            return Try(String($"{JassSymbol.Slash}{JassSymbol.Slash}")).Then(AnyCharExcept(JassSymbol.CarriageReturn, JassSymbol.LineFeed).ManyString());
        }

        internal static Parser<char, JassCommentSyntax> GetCommentParser(Parser<char, string> commentStringParser)
        {
            return commentStringParser.Select(comment => new JassCommentSyntax(comment));
        }
    }
}