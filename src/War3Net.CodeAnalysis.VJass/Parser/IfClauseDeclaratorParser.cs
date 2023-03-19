﻿// ------------------------------------------------------------------------------
// <copyright file="IfClauseDeclaratorParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassIfClauseDeclaratorSyntax> GetIfClauseDeclaratorParser(
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (ifToken, condition, thenToken) => new VJassIfClauseDeclaratorSyntax(
                    ifToken,
                    condition,
                    thenToken),
                Keyword.If.AsToken(triviaParser, VJassSyntaxKind.IfKeyword),
                expressionParser,
                Keyword.Then.AsToken(trailingTriviaParser, VJassSyntaxKind.ThenKeyword));
        }
    }
}