// ------------------------------------------------------------------------------
// <copyright file="Many1Parser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

// #define MANY1_PARSE_FRINGE_FIFO
// #define MANY1_PARSE_NO_YIELD
#define MANY1_PARSE_GREEDY

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A parser combinator that implements the unary + operator.
    /// </summary>
    internal class Many1Parser : IParser
    {
        private IParser _baseParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="Many1Parser"/> class.
        /// </summary>
        public Many1Parser(IParser baseParser = null)
        {
            _baseParser = baseParser;
        }

        public void SetParser(IParser baseParser)
        {
            if (_baseParser is null)
            {
                _baseParser = baseParser ?? throw new ArgumentNullException(nameof(baseParser));
            }
            else
            {
                throw new InvalidOperationException("Base parser cannot be overwritten after it's been set.");
            }
        }

#if MANY1_PARSE_GREEDY
        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            foreach (var (nodes, consumedTokens) in Parse(state, new List<SyntaxNode>(), 0))
            {
                yield return new ParseResult(CreateNode(nodes), consumedTokens);
            }
        }

        private IEnumerable<(List<SyntaxNode> nodes, int consumedTokens)> Parse(ParseState state, List<SyntaxNode> currentNodes, int consumedTokens)
        {
            foreach (var result in _baseParser.Parse(state))
            {
                var totalTokensConsumed = result.ConsumedTokens + consumedTokens;
                var newState = new ParseState();
                newState.Position = state.Position + result.ConsumedTokens;
                newState.Tokens = state.Tokens;

                var newNodes = new List<SyntaxNode>(currentNodes);
                newNodes.Add(result.Node);

                foreach (var newResult in Parse(newState, newNodes, totalTokensConsumed))
                {
                    yield return newResult;
                }

                yield return (new List<SyntaxNode>(newNodes), totalTokensConsumed);
            }
        }
#else
        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            var startPosition = state.Position;

#if MANY1_PARSE_FRINGE_FIFO
            var fringe = new Queue<(SyntaxNode[] nodes, ParseState state)>();
            fringe.Enqueue((Array.Empty<SyntaxNode>(), state));
#else
            var fringe = new Stack<(SyntaxNode[] nodes, ParseState state)>();
            fringe.Push((Array.Empty<SyntaxNode>(), state));
#endif

#if MANY1_PARSE_NO_YIELD
            var results = new Stack<ParseResult>();
#endif

            while (fringe.Count > 0)
            {
#if MANY1_PARSE_FRINGE_FIFO
                var next = fringe.Dequeue();
#else
                var next = fringe.Pop();
#endif

                foreach (var subResult in _baseParser.Parse(next.state))
                {
                    var newState = new ParseState();
                    newState.Position = next.state.Position + subResult.ConsumedTokens;
                    newState.Tokens = next.state.Tokens;

                    var newNodes = new SyntaxNode[next.nodes.Length + 1];
                    next.nodes.CopyTo(newNodes, 0);
                    newNodes[newNodes.Length - 1] = subResult.Node;

#if MANY1_PARSE_NO_YIELD
                    results.Push(new ParseResult(CreateNode(newNodes), newState.Position - startPosition));
#else
                    yield return new ParseResult(CreateNode(newNodes), newState.Position - startPosition);
#endif

#if MANY1_PARSE_FRINGE_FIFO
                    fringe.Enqueue((newNodes, newState));
#else
                    fringe.Push((newNodes, newState));
#endif
                }
            }

#if MANY1_PARSE_NO_YIELD
            while (results.Count > 0)
            {
                yield return results.Pop();
            }
#endif
        }
#endif

        protected virtual SyntaxNode CreateNode(params SyntaxNode[] nodes)
        {
            return new UnnamedNode(nodes);
        }

        private SyntaxNode CreateNode(List<SyntaxNode> nodes)
        {
            return CreateNode(nodes.ToArray());
        }
    }
}