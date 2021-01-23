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
        internal static Parser<char, IDeclarationSyntax> GetTypeDeclarationParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser)
        {
            return Keyword.Type.Then(identifierNameParser).Then(
                Keyword.Extends.Then(typeParser),
                (@new, @base) => (IDeclarationSyntax)new JassTypeDeclarationSyntax(@new, @base));
        }
    }
}