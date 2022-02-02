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
        internal static Parser<char, JassNativeFunctionDeclarationSyntax> GetNativeFunctionDeclarationParser(Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser)
        {
            return Try(Keyword.Constant.Optional().Then(Keyword.Native)).Then(functionDeclaratorParser)
                .Select(functionDeclaration => new JassNativeFunctionDeclarationSyntax(functionDeclaration));
        }
    }
}