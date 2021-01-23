// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorParser.cs" company="Drake53">
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
        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetUnaryOperatorParser(Parser<char, UnaryOperatorType> operatorTypeParser)
        {
            return operatorTypeParser.Select<Func<IExpressionSyntax, IExpressionSyntax>>(@operator => expression => new JassUnaryExpressionSyntax(@operator, expression));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetUnaryPlusOperatorParser()
        {
            return GetUnaryOperatorParser(Symbol.PlusSign.ThenReturn(UnaryOperatorType.Plus));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetUnaryMinusOperatorParser()
        {
            return GetUnaryOperatorParser(Symbol.MinusSign.ThenReturn(UnaryOperatorType.Minus));
        }

        internal static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> GetUnaryNotOperatorParser()
        {
            return GetUnaryOperatorParser(Keyword.Not.ThenReturn(UnaryOperatorType.Not));
        }
    }
}