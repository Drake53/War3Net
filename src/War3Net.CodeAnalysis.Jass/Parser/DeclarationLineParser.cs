// ------------------------------------------------------------------------------
// <copyright file="DeclarationLineParser.cs" company="Drake53">
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
        internal static Parser<char, IDeclarationLineSyntax> GetDeclarationLineParser(
            Parser<char, JassEmptySyntax> emptyLineParser,
            Parser<char, JassCommentSyntax> commentParser,
            Parser<char, JassTypeDeclarationSyntax> typeDeclarationParser,
            Parser<char, JassNativeFunctionDeclarationSyntax> nativeFunctionDeclarationParser,
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, Unit> whitespaceParser)
        {
            return OneOf(
                emptyLineParser.Cast<IDeclarationLineSyntax>(),
                commentParser.Cast<IDeclarationLineSyntax>(),
                typeDeclarationParser.Cast<IDeclarationLineSyntax>(),
                GetGlobalsCustomScriptActionParser(whitespaceParser),
                nativeFunctionDeclarationParser.Cast<IDeclarationLineSyntax>(),
                GetFunctionCustomScriptActionParser(functionDeclaratorParser, whitespaceParser));
        }
    }
}