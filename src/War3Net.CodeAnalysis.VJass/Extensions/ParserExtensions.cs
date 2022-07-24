// ------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass.Extensions
{
    internal static class ParserExtensions
    {
        internal static Parser<char, Func<VJassExpressionSyntax, VJassExpressionSyntax>> Prefix(
            this Parser<char, VJassSyntaxToken> operatorTokenParser)
        {
            return operatorTokenParser.Select<Func<VJassExpressionSyntax, VJassExpressionSyntax>>(operatorToken => expression => new VJassUnaryExpressionSyntax(
                operatorToken,
                expression));
        }

        internal static Parser<char, Func<VJassExpressionSyntax, VJassExpressionSyntax, VJassExpressionSyntax>> Infix(
            this Parser<char, VJassSyntaxToken> operatorTokenParser)
        {
            return operatorTokenParser.Select<Func<VJassExpressionSyntax, VJassExpressionSyntax, VJassExpressionSyntax>>(operatorToken => (left, right) => new VJassBinaryExpressionSyntax(
                left,
                operatorToken,
                right));
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, string> tokenSymbolParser,
            VJassSyntaxKind syntaxKind,
            string symbol)
        {
            return tokenSymbolParser.ThenReturn(new VJassSyntaxToken(syntaxKind, symbol, VJassSyntaxTriviaList.Empty));
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, char> tokenSymbolParser,
            Parser<char, ISyntaxTrivia> trailingTriviaParser,
            VJassSyntaxKind syntaxKind,
            string symbol)
        {
            return Map(
                (_, trailingTrivia) => new VJassSyntaxToken(
                    syntaxKind,
                    symbol,
                    new VJassSyntaxTriviaList(ImmutableArray.Create(trailingTrivia))),
                tokenSymbolParser,
                trailingTriviaParser);
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, char> tokenSymbolParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser,
            VJassSyntaxKind syntaxKind,
            string symbol)
        {
            return Map(
                (_, trailingTrivia) => new VJassSyntaxToken(
                    syntaxKind,
                    symbol,
                    trailingTrivia),
                tokenSymbolParser,
                trailingTriviaParser);
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, string> tokenTextParser,
            VJassSyntaxKind syntaxKind)
        {
            return tokenTextParser.Map(text => new VJassSyntaxToken(syntaxKind, text, VJassSyntaxTriviaList.Empty));
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, string> tokenTextParser,
            Parser<char, ISyntaxTrivia> trailingTriviaParser,
            VJassSyntaxKind syntaxKind)
        {
            return Map(
                (text, trailingTrivia) => new VJassSyntaxToken(
                    syntaxKind,
                    text,
                    new VJassSyntaxTriviaList(ImmutableArray.Create(trailingTrivia))),
                tokenTextParser,
                trailingTriviaParser);
        }

        internal static Parser<char, VJassSyntaxToken> AsToken(
            this Parser<char, string> tokenTextParser,
            Parser<char, VJassSyntaxTriviaList> trailingTriviaParser,
            VJassSyntaxKind syntaxKind)
        {
            return Map(
                (text, trailingTrivia) => new VJassSyntaxToken(
                    syntaxKind,
                    text,
                    trailingTrivia),
                tokenTextParser,
                trailingTriviaParser);
        }
    }
}