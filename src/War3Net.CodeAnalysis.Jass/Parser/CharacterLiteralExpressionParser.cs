// ------------------------------------------------------------------------------
// <copyright file="CharacterLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetCharacterLiteralExpressionParser()
        {
            var escapeCharacterParser = OneOf(
                Symbol.QuotationMark.ThenReturn(JassSymbol.QuotationMark),
                Symbol.Apostrophe.ThenReturn(JassSymbol.Apostrophe),
                Char('r').ThenReturn('\r'),
                Char('n').ThenReturn('\n'),
                Char('t').ThenReturn('\t'),
                Char('b').ThenReturn('\b'),
                Char('f').ThenReturn('\f'),
                Char('\\').ThenReturn('\\'));

            return Try(Char('\\').Then(escapeCharacterParser).Or(AnyCharExcept(JassSymbol.Apostrophe)).Between(Symbol.Apostrophe))
                .Select<IExpressionSyntax>(value => new JassCharacterLiteralExpressionSyntax(value))
                .Labelled("character literal");
        }
    }
}