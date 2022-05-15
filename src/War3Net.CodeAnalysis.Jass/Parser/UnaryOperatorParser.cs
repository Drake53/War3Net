// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorParser.cs" company="Drake53">
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
        internal static Parser<char, UnaryOperatorType> GetUnaryOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return OneOf(
                GetUnaryPlusOperatorParser(whitespaceParser),
                GetUnaryMinusOperatorParser(whitespaceParser),
                GetUnaryNotOperatorParser());
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryPlusOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.PlusSign.Before(whitespaceParser).ThenReturn(UnaryOperatorType.Plus);
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryMinusOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.MinusSign.Before(whitespaceParser).ThenReturn(UnaryOperatorType.Minus);
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryNotOperatorParser()
        {
            return Keyword.Not.ThenReturn(UnaryOperatorType.Not);
        }
    }
}