// ------------------------------------------------------------------------------
// <copyright file="NullLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetNullLiteralExpressionParser(Parser<char, Unit> whitespaceParser)
        {
            return Keyword.Null.Then(whitespaceParser).ThenReturn<IExpressionSyntax>(JassNullLiteralExpressionSyntax.Value)
                .Labelled("null literal");
        }
    }
}