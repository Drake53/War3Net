// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, JassFunctionDeclaratorSyntax> GetFunctionDeclaratorParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassParameterListSyntax> parameterListParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Map(
                (id, parameterList, returnType) => new JassFunctionDeclaratorSyntax(id, parameterList, returnType),
                identifierNameParser,
                Keyword.Takes.Then(whitespaceParser).Then(Keyword.Nothing.Then(whitespaceParser).ThenReturn(JassParameterListSyntax.Empty).Or(parameterListParser)),
                Keyword.Returns.Then(whitespaceParser).Then(Keyword.Nothing.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Nothing).Or(typeParser)));
        }
    }
}