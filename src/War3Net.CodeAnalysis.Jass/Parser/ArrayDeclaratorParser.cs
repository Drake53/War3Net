// ------------------------------------------------------------------------------
// <copyright file="ArrayDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, IVariableDeclarator> GetArrayDeclaratorParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser)
        {
            return Map(
                (type, id) => (IVariableDeclarator)new JassArrayDeclaratorSyntax(type, id),
                Try(typeParser.Before(Keyword.Array)),
                identifierNameParser);
        }
    }
}