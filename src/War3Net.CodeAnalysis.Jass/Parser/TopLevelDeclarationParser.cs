// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTopLevelDeclarationSyntax> GetTopLevelDeclarationParser(
            Parser<char, JassTopLevelDeclarationSyntax> typeDeclarationParser,
            Parser<char, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>> nativeFunctionDeclarationParser,
            Parser<char, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>> functionDeclarationParser,
            Parser<char, JassGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return OneOf(
                typeDeclarationParser,
                GetGlobalsDeclarationParser(globalDeclarationParser, leadingTriviaParser, trailingTriviaParser),
                Map(
                    (constantToken, declarationFunc) => declarationFunc(constantToken),
                    Keyword.Constant.AsToken(triviaParser, JassSyntaxKind.ConstantKeyword).Optional(),
                    OneOf(
                        nativeFunctionDeclarationParser,
                        functionDeclarationParser)));
        }
    }
}