// ------------------------------------------------------------------------------
// <copyright file="StringLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetStringLiteralExpressionParser()
        {
            var escapeSequenceParser = OneOf(
                Symbol.QuotationMark.ThenReturn($"\\{JassSymbol.QuotationMark}"),
                Symbol.Apostrophe.ThenReturn($"\\{JassSymbol.Apostrophe}"),
                Char('r').ThenReturn("\\r"),
                Char('n').ThenReturn("\\n"),
                Char('t').ThenReturn("\\t"),
                Char('b').ThenReturn("\\b"),
                Char('f').ThenReturn("\\f"),
                Char('\\').ThenReturn("\\\\"),
                Any.Then(c => Fail<string>($"\"\\{c}\" is not a valid escape sequence")))
                .Labelled("escape sequence");

            return Char('\\').Then(escapeSequenceParser).Or(AnyCharExcept(JassSymbol.QuotationMark).Map(char.ToString)).ManyString().Between(Symbol.QuotationMark)
                .Select<IExpressionSyntax>(value => new JassStringLiteralExpressionSyntax(value))
                .Labelled("string literal");
        }
    }
}