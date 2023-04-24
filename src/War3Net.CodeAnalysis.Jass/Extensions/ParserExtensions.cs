// ------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    internal static class ParserExtensions
    {
        internal static Parser<char, Func<JassExpressionSyntax, JassExpressionSyntax>> Prefix(
            this Parser<char, JassSyntaxToken> operatorTokenParser)
        {
            return operatorTokenParser.Select<Func<JassExpressionSyntax, JassExpressionSyntax>>(operatorToken => expression => new JassUnaryExpressionSyntax(
                operatorToken,
                expression));
        }

        internal static Parser<char, Func<JassExpressionSyntax, JassExpressionSyntax, JassExpressionSyntax>> Infix(
            this Parser<char, JassSyntaxToken> operatorTokenParser)
        {
            return operatorTokenParser.Select<Func<JassExpressionSyntax, JassExpressionSyntax, JassExpressionSyntax>>(operatorToken => (left, right) => new JassBinaryExpressionSyntax(
                left,
                operatorToken,
                right));
        }

        internal static Parser<char, JassSyntaxToken> AsToken(
            this Parser<char, char> tokenSymbolParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser,
            JassSyntaxKind syntaxKind,
            string symbol)
        {
            return Map(
                (_, trailingTrivia) => new JassSyntaxToken(
                    syntaxKind,
                    symbol,
                    trailingTrivia),
                tokenSymbolParser,
                trailingTriviaParser);
        }

        internal static Parser<char, JassSyntaxToken> AsToken(
            this Parser<char, string> tokenTextParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser,
            JassSyntaxKind syntaxKind)
        {
            return Map(
                (text, trailingTrivia) => new JassSyntaxToken(
                    syntaxKind,
                    text,
                    trailingTrivia),
                tokenTextParser,
                trailingTriviaParser);
        }
    }
}