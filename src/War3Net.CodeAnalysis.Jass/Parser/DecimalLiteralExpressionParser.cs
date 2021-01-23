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
            return Try(UnsignedInt(10))
                .Select<IExpressionSyntax>(value => new JassDecimalLiteralExpressionSyntax(value))
                .Labelled("decimal literal");
        }
    }
}