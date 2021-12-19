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
        internal static Parser<char, UnaryOperatorType> GetUnaryOperatorParser()
        {
            return OneOf(
                GetUnaryPlusOperatorParser(),
                GetUnaryMinusOperatorParser(),
                GetUnaryNotOperatorParser());
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryPlusOperatorParser()
        {
            return Symbol.PlusSign.ThenReturn(UnaryOperatorType.Plus);
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryMinusOperatorParser()
        {
            return Symbol.MinusSign.ThenReturn(UnaryOperatorType.Minus);
        }

        internal static Parser<char, UnaryOperatorType> GetUnaryNotOperatorParser()
        {
            return Keyword.Not.ThenReturn(UnaryOperatorType.Not);
        }
    }
}