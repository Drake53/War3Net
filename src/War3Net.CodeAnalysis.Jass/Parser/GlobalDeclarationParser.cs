// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, IDeclarationSyntax> GetGlobalDeclarationParser(
            Parser<char, IDeclarationSyntax> emptyDeclarationParser,
            Parser<char, IDeclarationSyntax> commentDeclarationParser,
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, Unit> endOfLineParser)
        {
            var constantDeclaratorParser = Keyword.Constant.Then(Map(
                (type, id, value) => (IDeclarationSyntax)new JassGlobalDeclarationSyntax(new JassVariableDeclaratorSyntax(type, id, value)),
                typeParser,
                identifierNameParser,
                equalsValueClauseParser));

            var variableDeclaratorParser = GetVariableDeclaratorParser(equalsValueClauseParser, identifierNameParser, Try(typeParser))
                .Select<IDeclarationSyntax>(declarator => new JassGlobalDeclarationSyntax(declarator));

            return OneOf(
                emptyDeclarationParser,
                commentDeclarationParser,
                constantDeclaratorParser,
                variableDeclaratorParser)
                .Before(endOfLineParser);
        }
    }
}