// ------------------------------------------------------------------------------
// <copyright file="ConstantDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, VJassGlobalDeclarationSyntax> GetConstantDeclarationParser(
            Parser<char, VJassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassTypeSyntax> typeParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (constantToken, type, id, value) => (VJassGlobalDeclarationSyntax)new VJassGlobalVariableDeclarationSyntax(new VJassVariableDeclaratorSyntax(type, id, value)),
                Keyword.Constant.AsToken(triviaParser, VJassSyntaxKind.ConstantKeyword),
                typeParser,
                identifierNameParser,
                equalsValueClauseParser);
        }
    }
}