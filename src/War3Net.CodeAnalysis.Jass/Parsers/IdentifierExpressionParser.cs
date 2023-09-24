// ------------------------------------------------------------------------------
// <copyright file="IdentifierExpressionParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Parsers
{
    internal sealed class IdentifierExpressionParser : Parser<char, JassExpressionSyntax>
    {
        private static readonly Dictionary<string, JassSyntaxKind> _keywordLiteralExpressionKinds = GetKeywordLiteralExpressionKinds();

        private readonly Parser<char, string> _identifierParser;
        private readonly Parser<char, JassIdentifierNameSyntax> _identifierNameParser;
        private readonly Parser<char, JassArgumentListSyntax> _argumentListParser;
        private readonly Parser<char, JassElementAccessClauseSyntax> _elementAccessClauseParser;
        private readonly Parser<char, JassSyntaxTriviaList> _triviaParser;

        public IdentifierExpressionParser(
            Parser<char, string> identifierParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassArgumentListSyntax> argumentListParser,
            Parser<char, JassElementAccessClauseSyntax> elementAccessClauseParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            _identifierParser = identifierParser;
            _identifierNameParser = identifierNameParser;
            _argumentListParser = argumentListParser;
            _elementAccessClauseParser = elementAccessClauseParser;
            _triviaParser = triviaParser;
        }

        public override bool TryParse(
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            [MaybeNullWhen(false)] out JassExpressionSyntax result)
        {
            var childExpecteds = new PooledList<Expected<char>>(state.Configuration.ArrayPoolProvider.GetArrayPool<Expected<char>>());

            if (!_identifierParser.TryParse(ref state, ref childExpecteds, out var identifierResult))
            {
                expecteds.AddRange(childExpecteds.AsSpan());
                childExpecteds.Dispose();
                result = null;
                return false;
            }

            childExpecteds.Clear();

            if (!_triviaParser.TryParse(ref state, ref childExpecteds, out var identifierTriviaResult))
            {
                expecteds.AddRange(childExpecteds.AsSpan());
                childExpecteds.Dispose();
                result = null;
                return false;
            }

            childExpecteds.Clear();

            if (_keywordLiteralExpressionKinds.TryGetValue(identifierResult, out var keywordLiteralExpressionKind))
            {
                childExpecteds.Dispose();
                result = new JassLiteralExpressionSyntax(new JassSyntaxToken(keywordLiteralExpressionKind, identifierResult, identifierTriviaResult));
                return true;
            }

            if (string.Equals(identifierResult, JassKeyword.Function, StringComparison.Ordinal))
            {
                if (!_identifierNameParser.TryParse(ref state, ref childExpecteds, out var identifierNameResult))
                {
                    expecteds.AddRange(childExpecteds.AsSpan());
                    childExpecteds.Dispose();
                    result = null;
                    return false;
                }

                childExpecteds.Dispose();
                result = new JassFunctionReferenceExpressionSyntax(
                    new JassSyntaxToken(JassSyntaxKind.FunctionKeyword, JassKeyword.Function, identifierTriviaResult),
                    identifierNameResult);

                return true;
            }

            var identifierStartLoc = state.Location;

            if (_argumentListParser.TryParse(ref state, ref childExpecteds, out var argumentListResult))
            {
                childExpecteds.Dispose();
                result = new JassInvocationExpressionSyntax(
                    new JassIdentifierNameSyntax(new JassSyntaxToken(JassSyntaxKind.IdentifierToken, identifierResult, identifierTriviaResult)),
                    argumentListResult);

                return true;
            }
            else if (state.Location > identifierStartLoc)
            {
                expecteds.AddRange(childExpecteds.AsSpan());
                childExpecteds.Dispose();
                result = null;
                return false;
            }

            childExpecteds.Clear();

            if (_elementAccessClauseParser.TryParse(ref state, ref childExpecteds, out var elementAccessClauseResult))
            {
                childExpecteds.Dispose();
                result = new JassElementAccessExpressionSyntax(
                    new JassIdentifierNameSyntax(new JassSyntaxToken(JassSyntaxKind.IdentifierToken, identifierResult, identifierTriviaResult)),
                    elementAccessClauseResult);

                return true;
            }
            else if (state.Location > identifierStartLoc)
            {
                expecteds.AddRange(childExpecteds.AsSpan());
                childExpecteds.Dispose();
                result = null;
                return false;
            }

            childExpecteds.Dispose();
            result = new JassIdentifierNameSyntax(new JassSyntaxToken(JassSyntaxKind.IdentifierToken, identifierResult, identifierTriviaResult));
            return true;
        }

        private static Dictionary<string, JassSyntaxKind> GetKeywordLiteralExpressionKinds()
        {
            return new Dictionary<string, JassSyntaxKind>(StringComparer.Ordinal)
            {
                { JassKeyword.True, JassSyntaxKind.TrueKeyword },
                { JassKeyword.False, JassSyntaxKind.FalseKeyword },
                { JassKeyword.Null, JassSyntaxKind.NullKeyword },
            };
        }
    }
}