// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>> GetNativeFunctionDeclarationParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassParameterListOrEmptyParameterListSyntax> parameterListParser,
            Parser<char, JassReturnClauseSyntax> returnClauseParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map<char, JassSyntaxToken, JassIdentifierNameSyntax, JassParameterListOrEmptyParameterListSyntax, JassReturnClauseSyntax, JassSyntaxTriviaList, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>>(
                (nativeToken, identifierName, parameterList, returnClause, trailingTrivia) => constantToken => new JassNativeFunctionDeclarationSyntax(
                    constantToken.GetValueOrDefault(),
                    nativeToken,
                    identifierName,
                    parameterList,
                    returnClause.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Native.AsToken(triviaParser, JassSyntaxKind.NativeKeyword),
                identifierNameParser,
                parameterListParser,
                returnClauseParser,
                trailingTriviaParser);
        }
    }
}