// ------------------------------------------------------------------------------
// <copyright file="ReturnClauseParser.cs" company="Drake53">
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
        internal static Parser<char, JassReturnClauseSyntax> GetReturnClauseParser(
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (returnsToken, type, trailingTrivia) => new JassReturnClauseSyntax(
                    returnsToken,
                    type.AppendTrivia(trailingTrivia)),
                Keyword.Returns.AsToken(triviaParser, JassSyntaxKind.ReturnsKeyword),
                typeParser,
                trailingTriviaParser);
        }
    }
}