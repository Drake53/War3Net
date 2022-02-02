// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassGlobalDeclarationSyntax> GetVariableDeclarationParser(
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser)
        {
            return GetVariableDeclaratorParser(equalsValueClauseParser, identifierNameParser, Try(typeParser))
                .Select(declarator => new JassGlobalDeclarationSyntax(declarator));
        }
    }
}