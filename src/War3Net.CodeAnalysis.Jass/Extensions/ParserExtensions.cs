// ------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class ParserExtensions
    {
        public static Parser<char, Func<IExpressionSyntax, IExpressionSyntax>> Prefix(
            this Parser<char, UnaryOperatorType> parser)
        {
            return parser.Select<Func<IExpressionSyntax, IExpressionSyntax>>(@operator => expression => new JassUnaryExpressionSyntax(@operator, expression));
        }

        public static Parser<char, Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>> Infix(
            this Parser<char, BinaryOperatorType> parser)
        {
            return parser.Select<Func<IExpressionSyntax, IExpressionSyntax, IExpressionSyntax>>(@operator => (left, right) => new JassBinaryExpressionSyntax(@operator, left, right));
        }
    }
}