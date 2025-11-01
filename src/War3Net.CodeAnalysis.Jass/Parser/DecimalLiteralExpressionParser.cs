// ------------------------------------------------------------------------------
// <copyright file="DecimalLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetDecimalLiteralExpressionParser()
        {
            return Try(UnsignedLong(10)
                .Bind(value =>
                    value - 1 == int.MaxValue
                        ? Parser<char>.Return<IExpressionSyntax>(new JassDecimalLiteralExpressionSyntax(value))
                        : Parser<char>.Fail<IExpressionSyntax>("Edge case only. Fall-through to UnsignedInt")
                ))
                .Or(
                    Try(UnsignedInt(10)
                        .Select<IExpressionSyntax>(value => new JassDecimalLiteralExpressionSyntax(value)))
                )
                .Labelled("decimal literal");
       }
    }
}