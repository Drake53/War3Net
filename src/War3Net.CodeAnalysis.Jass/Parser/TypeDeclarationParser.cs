// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTypeDeclarationSyntax> GetTypeDeclarationParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Keyword.Type.Then(whitespaceParser).Then(identifierNameParser).Then(
                Keyword.Extends.Then(whitespaceParser).Then(typeParser),
                (@new, @base) => new JassTypeDeclarationSyntax(@new, @base));
        }
    }
}