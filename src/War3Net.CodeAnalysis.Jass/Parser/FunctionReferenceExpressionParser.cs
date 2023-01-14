// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassExpressionSyntax> GetFunctionReferenceExpressionParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (functionToken, identifierName) => (JassExpressionSyntax)new JassFunctionReferenceExpressionSyntax(
                    functionToken,
                    identifierName),
                Keyword.Function.AsToken(triviaParser, JassSyntaxKind.FunctionKeyword),
                identifierNameParser)
                .Labelled("function reference");
        }
    }
}