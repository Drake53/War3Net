// ------------------------------------------------------------------------------
// <copyright file="StructDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, VJassStructDeclaratorSyntax> GetStructDeclaratorParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassExtendsClauseSyntax> extendsClauseParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (structToken, identifierName, extendsClause, trailingTrivia) => new VJassStructDeclaratorSyntax(
                    structToken,
                    identifierName,
                    extendsClause.GetValueOrDefault()).AppendTrivia(trailingTrivia),
                Keyword.Struct.AsToken(triviaParser, VJassSyntaxKind.StructKeyword),
                identifierNameParser.Labelled("struct identifier name"),
                extendsClauseParser.Optional(),
                trailingTriviaParser);
        }
    }
}