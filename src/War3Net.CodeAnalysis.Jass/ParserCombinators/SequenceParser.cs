// ------------------------------------------------------------------------------
// <copyright file="SequenceParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A parser combinator that runs its base parsers in the order in which they were added.
    /// It only succeeds none of the base parsers failed.
    /// </summary>
    internal abstract class SequenceParser : IParser
    {
        private List<IParser> _baseParsers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceParser"/> class.
        /// </summary>
        public SequenceParser(params IParser[] baseParsers)
        {
            _baseParsers = new List<IParser>(baseParsers);
        }

        public void AddParser(IParser baseParser)
        {
            _baseParsers.Add(baseParser ?? throw new ArgumentNullException(nameof(baseParser)));
        }

        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            foreach (var (nodes, consumedTokens) in Parse(state, _baseParsers.Count - 1))
            {
                yield return new ParseResult(CreateNode(nodes), consumedTokens);
            }
        }

        protected abstract SyntaxNode CreateNode(List<SyntaxNode> nodes);

        private IEnumerable<(List<SyntaxNode> nodes, int consumedTokens)> Parse(ParseState state, int offset)
        {
            if (offset == 0)
            {
                foreach (var result in _baseParsers[offset].Parse(state))
                {
                    var nodes = new SyntaxNode[] { result.Node };
                    yield return (new List<SyntaxNode>(nodes), result.ConsumedTokens);
                }
            }
            else
            {
                foreach (var head in Parse(state, offset - 1))
                {
                    var newState = new ParseState();
                    newState.Position = state.Position + head.consumedTokens;
                    newState.Tokens = state.Tokens;

                    var test = 0;
                    foreach (var result in _baseParsers[offset].Parse(newState))
                    {
                        test++;
                        var nodes = new SyntaxNode[head.nodes.Count + 1];
                        head.nodes.CopyTo(nodes);
                        nodes[head.nodes.Count] = result.Node;

                        yield return (new List<SyntaxNode>(nodes), head.consumedTokens + result.ConsumedTokens);
                    }

                    if (test == 0 && head.consumedTokens > 0)
                    {
                        var self = GetType().DeclaringType.Name;
                        var subParser = _baseParsers[offset];
                        var sub = subParser is TokenParser tokenParser
                            ? tokenParser.TokenType.ToString()
                            : subParser.GetType().DeclaringType.Name;

                        if (newState.Position == 7232)
                        {
                            Console.WriteLine($"[Token {newState.Position}]: Sequence parser {self} broke sequence at {sub} ({offset + 1}/{_baseParsers.Count})");
                        }
                    }
                }
            }
        }
    }
}