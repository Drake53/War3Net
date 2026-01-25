// ------------------------------------------------------------------------------
// <copyright file="VariableOrArrayDeclaratorParser.cs" company="Drake53">
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
        internal static Parser<char, JassVariableOrArrayDeclaratorSyntax> GetVariableOrArrayDeclaratorParser(
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (type, declaratorFunc) => declaratorFunc(type),
                typeParser,
                OneOf(
                    Map<char, JassSyntaxToken, JassIdentifierNameSyntax, Func<JassTypeSyntax, JassVariableOrArrayDeclaratorSyntax>>(
                        (arrayToken, identifierName) => type => new JassArrayDeclaratorSyntax(
                            type,
                            arrayToken,
                            identifierName),
                        Keyword.Array.AsToken(triviaParser, JassSyntaxKind.ArrayKeyword),
                        identifierNameParser),
                    Map<char, JassIdentifierNameSyntax, Maybe<JassEqualsValueClauseSyntax>, Func<JassTypeSyntax, JassVariableOrArrayDeclaratorSyntax>>(
                        (identifierName, value) => type => new JassVariableDeclaratorSyntax(
                            type,
                            identifierName,
                            value.GetValueOrDefault()),
                        identifierNameParser,
                        equalsValueClauseParser.Optional())));
        }
    }
}