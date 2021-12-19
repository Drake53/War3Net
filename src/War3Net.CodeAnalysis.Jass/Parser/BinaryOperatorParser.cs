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
        internal static Parser<char, BinaryOperatorType> GetBinaryOperatorParser()
        {
            return OneOf(
                GetBinaryAddOperatorParser(),
                GetBinarySubtractOperatorParser(),
                GetBinaryMultiplicationOperatorParser(),
                GetBinaryDivisionOperatorParser(),
                GetBinaryGreaterOrEqualOperatorParser(),
                GetBinaryLessOrEqualOperatorParser(),
                GetBinaryEqualsOperatorParser(),
                GetBinaryNotEqualsOperatorParser(),
                GetBinaryGreaterThanOperatorParser(),
                GetBinaryLessThanOperatorParser(),
                GetBinaryAndOperatorParser(),
                GetBinaryOrOperatorParser());
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryAddOperatorParser()
        {
            return Symbol.PlusSign.ThenReturn(BinaryOperatorType.Add);
        }

        internal static Parser<char, BinaryOperatorType> GetBinarySubtractOperatorParser()
        {
            return Symbol.MinusSign.ThenReturn(BinaryOperatorType.Subtract);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryMultiplicationOperatorParser()
        {
            return Symbol.Asterisk.ThenReturn(BinaryOperatorType.Multiplication);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryDivisionOperatorParser()
        {
            return Symbol.Slash.ThenReturn(BinaryOperatorType.Division);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryGreaterOrEqualOperatorParser()
        {
            return Symbol.GreaterOrEquals.ThenReturn(BinaryOperatorType.GreaterOrEqual);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryLessOrEqualOperatorParser()
        {
            return Symbol.LessOrEquals.ThenReturn(BinaryOperatorType.LessOrEqual);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryEqualsOperatorParser()
        {
            return Symbol.EqualsEquals.ThenReturn(BinaryOperatorType.Equals);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryNotEqualsOperatorParser()
        {
            return Symbol.NotEquals.ThenReturn(BinaryOperatorType.NotEquals);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryGreaterThanOperatorParser()
        {
            return Symbol.GreaterThanSign.ThenReturn(BinaryOperatorType.GreaterThan);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryLessThanOperatorParser()
        {
            return Symbol.LessThanSign.ThenReturn(BinaryOperatorType.LessThan);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryAndOperatorParser()
        {
            return Keyword.And.ThenReturn(BinaryOperatorType.And);
        }

        internal static Parser<char, BinaryOperatorType> GetBinaryOrOperatorParser()
        {
            return Keyword.Or.ThenReturn(BinaryOperatorType.Or);
        }
    }
}