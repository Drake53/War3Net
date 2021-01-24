// ------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class ParserExtensions
    {
        public static Parser<char, T> SkipWhitespaces<T>(this Parser<char, T> parser)
        {
            return parser.Before(Token(@char => char.IsWhiteSpace(@char) && @char != JassSymbol.CarriageReturn && @char != JassSymbol.LineFeed).Many());
        }

        public static Parser<char, T> AssertNotFollowedByLetterOrDigitOrUnderscore<T>(this Parser<char, T> parser)
        {
            return parser.Before(Not(Token(@char => char.IsLetterOrDigit(@char) || @char == '_')));
        }
    }
}