// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryOperatorParser(Parser<char, BinaryOperatorType> operatorTypeParser)
        {
            return operatorTypeParser.Select<Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>>(@operator => (left, right) => new JassBinaryExpressionSyntax(@operator, left, right));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryAddOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.PlusSign.ThenReturn(BinaryOperatorType.Add));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinarySubtractOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.MinusSign.ThenReturn(BinaryOperatorType.Subtract));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryMultiplicationOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.Asterisk.ThenReturn(BinaryOperatorType.Multiplication));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryDivisionOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.Slash.ThenReturn(BinaryOperatorType.Division));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryGreaterOrEqualOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.GreaterOrEquals.ThenReturn(BinaryOperatorType.GreaterOrEqual));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryLessOrEqualOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.LessOrEquals.ThenReturn(BinaryOperatorType.LessOrEqual));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryEqualsOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.EqualsEquals.ThenReturn(BinaryOperatorType.Equals));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryNotEqualsOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.NotEquals.ThenReturn(BinaryOperatorType.NotEquals));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryGreaterThanOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.GreaterThanSign.ThenReturn(BinaryOperatorType.GreaterThan));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryLessThanOperatorParser()
        {
            return GetBinaryOperatorParser(Symbol.LessThanSign.ThenReturn(BinaryOperatorType.LessThan));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryAndOperatorParser()
        {
            return GetBinaryOperatorParser(Keyword.And.ThenReturn(BinaryOperatorType.And));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> GetBinaryOrOperatorParser()
        {
            return GetBinaryOperatorParser(Keyword.Or.ThenReturn(BinaryOperatorType.Or));
        }
    }
}