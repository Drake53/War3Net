// ------------------------------------------------------------------------------
// <copyright file="BooleanLiteralExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, JassExpressionSyntax> GetBooleanLiteralExpressionParser(
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return OneOf(
                Keyword.True.AsToken(triviaParser, JassSyntaxKind.TrueKeyword),
                Keyword.False.AsToken(triviaParser, JassSyntaxKind.FalseKeyword))
                .Select(token => (JassExpressionSyntax)new JassLiteralExpressionSyntax(token));
        }
    }
}