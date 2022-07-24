// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassExpressionSyntax> GetInvocationExpressionParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassArgumentListSyntax> argumentListParser,
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser)
        {
            return Map(
                (identifierName, argumentList) => (VJassExpressionSyntax)new VJassInvocationExpressionSyntax(
                    identifierName,
                    argumentList),
                Try(identifierNameParser.Before(Lookahead(Symbol.OpenParen))),
                argumentListParser);
        }
    }
}