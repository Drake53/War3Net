// ------------------------------------------------------------------------------
// <copyright file="DeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, IDeclarationSyntax> GetDeclarationParser(
            Parser<char, JassEmptySyntax> emptyParser,
            Parser<char, JassCommentSyntax> commentParser,
            Parser<char, JassTypeDeclarationSyntax> typeDeclarationParser,
            Parser<char, JassNativeFunctionDeclarationSyntax> nativeFunctionDeclarationParser,
            Parser<char, JassFunctionDeclarationSyntax> functionDeclarationParser,
            Parser<char, IGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, Unit> endOfLineParser)
        {
            return OneOf(
                emptyParser.Cast<IDeclarationSyntax>(),
                commentParser.Cast<IDeclarationSyntax>(),
                typeDeclarationParser.Cast<IDeclarationSyntax>(),
                GetGlobalDeclarationListParser(globalDeclarationParser, endOfLineParser),
                nativeFunctionDeclarationParser.Cast<IDeclarationSyntax>(),
                functionDeclarationParser.Cast<IDeclarationSyntax>());
        }
    }
}