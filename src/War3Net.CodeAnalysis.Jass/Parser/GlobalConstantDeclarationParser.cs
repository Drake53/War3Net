// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, JassGlobalDeclarationSyntax> GetGlobalConstantDeclarationParser(
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (constantToken, type, identifierName, value) => (JassGlobalDeclarationSyntax)new JassGlobalConstantDeclarationSyntax(constantToken, type, identifierName, value),
                Keyword.Constant.AsToken(triviaParser, JassSyntaxKind.ConstantKeyword),
                typeParser,
                identifierNameParser,
                equalsValueClauseParser);
        }
    }
}