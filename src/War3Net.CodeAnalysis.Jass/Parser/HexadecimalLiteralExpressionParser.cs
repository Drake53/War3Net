// ------------------------------------------------------------------------------
// <copyright file="HexadecimalLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetHexadecimalLiteralExpressionParser()
        {
            return Symbol.DollarSign.Or(Try(Symbol.Zero.Then(Symbol.X))).Then(UnsignedInt(16))
                .Select<IExpressionSyntax>(value => new JassHexadecimalLiteralExpressionSyntax(value))
                .Labelled("hexadecimal literal");
        }
    }
}