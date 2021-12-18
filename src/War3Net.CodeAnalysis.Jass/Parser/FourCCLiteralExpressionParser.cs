// ------------------------------------------------------------------------------
// <copyright file="FourCCLiteralExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, IExpressionSyntax> GetFourCCLiteralExpressionParser()
        {
            return Symbol.Apostrophe.Then(AnyCharExcept(JassSymbol.Apostrophe).ManyString()).Before(Symbol.Apostrophe)
                .Assert(value => value.IsJassRawcode())
                .Select<IExpressionSyntax>(value => new JassFourCCLiteralExpressionSyntax(value.FromJassRawcode()))
                .Labelled("fourCC literal");
        }
    }
}