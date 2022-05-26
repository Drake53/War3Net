// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, JassNativeFunctionDeclarationSyntax> GetNativeFunctionDeclarationParser(
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Try(Keyword.Constant.Then(whitespaceParser).Optional().Then(Keyword.Native.Then(whitespaceParser))).Then(functionDeclaratorParser)
                .Select(functionDeclaration => new JassNativeFunctionDeclarationSyntax(functionDeclaration));
        }
    }
}