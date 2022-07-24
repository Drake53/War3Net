// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassTypeDeclarationSyntax> GetTypeDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassTypeSyntax> typeParser,
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (typeToken, identifierName, extendsToken, type) => new VJassTypeDeclarationSyntax(
                    typeToken,
                    identifierName,
                    extendsToken,
                    type),
                Keyword.Type.AsToken(triviaParser, VJassSyntaxKind.TypeKeyword),
                identifierNameParser,
                Keyword.Extends.AsToken(triviaParser, VJassSyntaxKind.ExtendsKeyword),
                typeParser);
        }
    }
}