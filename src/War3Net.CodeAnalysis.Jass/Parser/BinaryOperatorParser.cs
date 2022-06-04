// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParser.cs" company="Drake53">
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
        internal static Parser<char, BinaryOperatorType> GetBinaryOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return OneOf(
                GetBinaryAddOperatorParser(whitespaceParser),
                GetBinarySubtractOperatorParser(whitespaceParser),
                GetBinaryMultiplicationOperatorParser(whitespaceParser),
                GetBinaryDivisionOperatorParser(whitespaceParser),
                GetBinaryGreaterOrEqualOperatorParser(whitespaceParser),
                GetBinaryLessOrEqualOperatorParser(whitespaceParser),
                GetBinaryEqualsOperatorParser(whitespaceParser),
                GetBinaryNotEqualsOperatorParser(whitespaceParser),
                GetBinaryGreaterThanOperatorParser(whitespaceParser),
                GetBinaryLessThanOperatorParser(whitespaceParser),
                GetBinaryAndOperatorParser(whitespaceParser),
                GetBinaryOrOperatorParser(whitespaceParser));
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryAddOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.PlusSign.Then(whitespaceParser).ThenReturn(BinaryOperatorType.Add);
        }

        internal static Parser<char, BinaryOperatorType> GetBinarySubtractOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.MinusSign.Then(whitespaceParser).ThenReturn(BinaryOperatorType.Subtract);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryMultiplicationOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.Asterisk.Then(whitespaceParser).ThenReturn(BinaryOperatorType.Multiplication);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryDivisionOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Try(Symbol.Slash.Then(Not(Lookahead(Symbol.Slash)))).Then(whitespaceParser).ThenReturn(BinaryOperatorType.Division);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryGreaterOrEqualOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Try(Symbol.GreaterOrEquals).Then(whitespaceParser).ThenReturn(BinaryOperatorType.GreaterOrEqual);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryLessOrEqualOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Try(Symbol.LessOrEquals).Then(whitespaceParser).ThenReturn(BinaryOperatorType.LessOrEqual);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryEqualsOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.EqualsEquals.Then(whitespaceParser).ThenReturn(BinaryOperatorType.Equals);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryNotEqualsOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.NotEquals.Then(whitespaceParser).ThenReturn(BinaryOperatorType.NotEquals);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryGreaterThanOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.GreaterThanSign.Then(whitespaceParser).ThenReturn(BinaryOperatorType.GreaterThan);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryLessThanOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Symbol.LessThanSign.Then(whitespaceParser).ThenReturn(BinaryOperatorType.LessThan);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryAndOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Keyword.And.Then(whitespaceParser).ThenReturn(BinaryOperatorType.And);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryOrOperatorParser(Parser<char, Unit> whitespaceParser)
        {
            return Keyword.Or.Then(whitespaceParser).ThenReturn(BinaryOperatorType.Or);
        }
    }
}