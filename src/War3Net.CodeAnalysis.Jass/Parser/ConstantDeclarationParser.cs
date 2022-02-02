// ------------------------------------------------------------------------------
// <copyright file="ConstantDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, JassGlobalDeclarationSyntax> GetConstantDeclarationParser(
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser)
        {
            return Keyword.Constant.Then(Map(
                (type, id, value) => new JassGlobalDeclarationSyntax(new JassVariableDeclaratorSyntax(type, id, value)),
                typeParser,
                identifierNameParser,
                equalsValueClauseParser));
        }
    }
}