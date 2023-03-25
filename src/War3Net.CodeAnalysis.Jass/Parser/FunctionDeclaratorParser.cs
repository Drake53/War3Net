// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, Func<Maybe<JassSyntaxToken>, JassFunctionDeclaratorSyntax>> GetFunctionDeclaratorParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassParameterListOrEmptyParameterListSyntax> parameterListParser,
            Parser<char, JassReturnClauseSyntax> returnClauseParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map<char, JassSyntaxToken, JassIdentifierNameSyntax, JassParameterListOrEmptyParameterListSyntax, JassReturnClauseSyntax, JassSyntaxTriviaList, Func<Maybe<JassSyntaxToken>, JassFunctionDeclaratorSyntax>>(
                (functionToken, identifierName, parameterList, returnClause, trailingTrivia) => constantToken => new JassFunctionDeclaratorSyntax(
                    constantToken.GetValueOrDefault(),
                    functionToken,
                    identifierName,
                    parameterList,
                    returnClause.AppendTrivia(trailingTrivia)),
                Keyword.Function.AsToken(triviaParser, JassSyntaxKind.FunctionKeyword),
                identifierNameParser,
                parameterListParser,
                returnClauseParser,
                trailingTriviaParser);
        }
    }
}