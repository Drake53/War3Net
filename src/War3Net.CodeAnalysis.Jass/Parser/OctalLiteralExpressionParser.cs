// ------------------------------------------------------------------------------
// <copyright file="OctalLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetOctalLiteralExpressionParser()
        {
            return Try(Symbol.Zero.Then(UnsignedInt(8).Optional()))
                .Select<IExpressionSyntax>(value => new JassOctalLiteralExpressionSyntax(value.GetValueOrDefault()))
                .Labelled("octal literal");
        }
    }
}