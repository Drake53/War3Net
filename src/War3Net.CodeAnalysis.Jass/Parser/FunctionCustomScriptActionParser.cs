// ------------------------------------------------------------------------------
// <copyright file="FunctionCustomScriptActionParser.cs" company="Drake53">
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
        internal static Parser<char, IDeclarationLineSyntax> GetFunctionCustomScriptActionParser(
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, Unit> whitespaceParser)
        {
            return Keyword.Function.Then(whitespaceParser).Then(functionDeclaratorParser)
                .Select<IDeclarationLineSyntax>(functionDeclarator => new JassFunctionCustomScriptAction(functionDeclarator));
        }
    }
}