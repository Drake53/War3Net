// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, VJassFunctionDeclaratorSyntax> GetFunctionDeclaratorParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassParameterListOrEmptyParameterListSyntax> parameterListParser,
            Parser<char, VJassReturnClauseSyntax> returnClauseParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (constantToken, functionToken, identifierName, parameterList, returnClause) => new VJassFunctionDeclaratorSyntax(
                    constantToken.GetValueOrDefault(),
                    functionToken,
                    identifierName,
                    parameterList,
                    returnClause),
                Keyword.Constant.AsToken(triviaParser, VJassSyntaxKind.ConstantKeyword).Optional(),
                Keyword.Function.AsToken(triviaParser, VJassSyntaxKind.FunctionKeyword),
                identifierNameParser,
                parameterListParser,
                returnClauseParser);
        }
    }
}