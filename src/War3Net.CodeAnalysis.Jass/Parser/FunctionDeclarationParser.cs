// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>> GetFunctionDeclarationParser(
            Parser<char, Func<Maybe<JassSyntaxToken>, JassFunctionDeclaratorSyntax>> functionDeclaratorParser,
            Parser<char, JassStatementSyntax> statementParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return statementParser.UntilWithLeading<char, JassSyntaxTriviaList, Func<Maybe<JassSyntaxToken>, JassFunctionDeclaratorSyntax>, JassStatementSyntax, JassSyntaxToken, Func<Maybe<JassSyntaxToken>, JassTopLevelDeclarationSyntax>>(
                leadingTriviaParser,
                functionDeclaratorParser,
                Keyword.EndFunction.AsToken(trailingTriviaParser, JassSyntaxKind.EndFunctionKeyword),
                (leadingTrivia, statement) => statement.WithLeadingTrivia(leadingTrivia),
                (declaratorFunc, statements, leadingTrivia, endFunctionToken) => constantToken => new JassFunctionDeclarationSyntax(
                    declaratorFunc(constantToken),
                    statements.ToImmutableArray(),
                    endFunctionToken.WithLeadingTrivia(leadingTrivia)));
        }
    }
}