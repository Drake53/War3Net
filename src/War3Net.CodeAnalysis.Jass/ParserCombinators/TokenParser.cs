// ------------------------------------------------------------------------------
// <copyright file="TokenParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// An atomic parser that consumes a single token of a specific <see cref="SyntaxTokenType"/>, and returns that token.
    /// </summary>
    internal sealed class TokenParser : IParser
    {
        private static readonly Lazy<Dictionary<SyntaxTokenType, TokenParser>> _cachedTokenParsers = new Lazy<Dictionary<SyntaxTokenType, TokenParser>>(
            () => new Dictionary<SyntaxTokenType, TokenParser>());

        private readonly SyntaxTokenType _tokenType;

        private TokenParser(SyntaxTokenType tokenType)
        {
            _cachedTokenParsers.Value.Add(tokenType, this);

            _tokenType = tokenType;
        }

        public SyntaxTokenType TokenType => _tokenType;

        public static TokenParser Get(SyntaxTokenType tokenType)
        {
            if (_cachedTokenParsers.Value.TryGetValue(tokenType, out var parser))
            {
                return parser;
            }

            return new TokenParser(tokenType);
        }

        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            var currentToken = (state.Position < state.Tokens.Count) ? (SyntaxToken?)state.Tokens[state.Position] : null;
            if ((currentToken?.TokenType ?? SyntaxTokenType.Undefined) == _tokenType)
            {
                yield return new ParseResult(new TokenNode(currentToken.Value, state.Position), 1);
            }
        }
    }
}