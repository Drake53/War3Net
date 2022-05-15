// ------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetParenthesizedExpressionParser(
            Parser<char, Unit> whitespaceParser,
            Parser<char, IExpressionSyntax> expressionParser)
        {
            return Symbol.LeftParenthesis.Before(whitespaceParser).Then(expressionParser).Before(Symbol.RightParenthesis.Before(whitespaceParser))
                .Select<IExpressionSyntax>(expression => new JassParenthesizedExpressionSyntax(expression))
                .Labelled("parenthesized expression");
        }
    }
}