// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassLocalVariableDeclarationStatementSyntax> GetLocalVariableDeclarationStatementParser(
            Parser<char, VJassVariableOrArrayDeclaratorSyntax> variableDeclaratorParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (localToken, declarator, trailingTrivia) => new VJassLocalVariableDeclarationStatementSyntax(
                    localToken,
                    declarator.AppendTrivia(trailingTrivia)),
                Keyword.Local.AsToken(triviaParser, VJassSyntaxKind.LocalKeyword),
                variableDeclaratorParser,
                trailingTriviaParser);
        }
    }
}