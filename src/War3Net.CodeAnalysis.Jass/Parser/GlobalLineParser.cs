// ------------------------------------------------------------------------------
// <copyright file="GlobalLineParser.cs" company="Drake53">
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
        internal static Parser<char, IGlobalLineSyntax> GetGlobalLineParser(
            Parser<char, JassEmptySyntax> emptyLineParser,
            Parser<char, JassCommentSyntax> commentParser,
            Parser<char, JassGlobalDeclarationSyntax> constantDeclarationParser,
            Parser<char, JassGlobalDeclarationSyntax> variableDeclarationParser,
            Parser<char, Unit> whitespaceParser)
        {
            return OneOf(
                emptyLineParser.Cast<IGlobalLineSyntax>(),
                commentParser.Cast<IGlobalLineSyntax>(),
                constantDeclarationParser.Cast<IGlobalLineSyntax>(),
                variableDeclarationParser.Cast<IGlobalLineSyntax>(),
                GetEndGlobalsCustomScriptActionParser(whitespaceParser));
        }
    }
}