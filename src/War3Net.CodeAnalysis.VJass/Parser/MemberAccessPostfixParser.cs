// ------------------------------------------------------------------------------
// <copyright file="MemberAccessPostfixParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, Func<VJassExpressionSyntax, VJassExpressionSyntax>> GetMemberAccessPostfixParser(
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassArgumentListSyntax> argumentListParser,
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser)
        {
            return Map<char, VJassSyntaxToken, VJassIdentifierNameSyntax, Maybe<VJassArgumentListSyntax>, Func<VJassExpressionSyntax, VJassExpressionSyntax>>(
                (dotToken, memberName, argumentList) => expression => argumentList.HasValue
                    ? new VJassInvocationExpressionSyntax(
                        new VJassMemberAccessExpressionSyntax(
                            expression,
                            dotToken,
                            memberName),
                        argumentList.Value)
                    : new VJassMemberAccessExpressionSyntax(
                        expression,
                        dotToken,
                        memberName),
                Symbol.Dot.AsToken(triviaParser, VJassSyntaxKind.DotToken, VJassSymbol.Dot),
                identifierNameParser.Labelled("member identifier name"),
                argumentListParser.Optional());
        }
    }
}