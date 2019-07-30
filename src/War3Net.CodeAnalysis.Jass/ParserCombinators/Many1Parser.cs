// ------------------------------------------------------------------------------
// <copyright file="Many1Parser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

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

        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            var startPosition = state.Position;
            var stack = new Stack<(SyntaxNode[] nodes, ParseState state)>();
            stack.Push((Array.Empty<SyntaxNode>(), state));

            while (stack.Count > 0)
            {
                var next = stack.Pop();

                foreach (var subResult in _baseParser.Parse(next.state))
                {
                    var newState = new ParseState();
                    newState.Position = next.state.Position + subResult.ConsumedTokens;
                    newState.Tokens = next.state.Tokens;

                    var newNodes = new SyntaxNode[next.nodes.Length + 1];
                    next.nodes.CopyTo(newNodes, 0);
                    newNodes[newNodes.Length - 1] = subResult.Node;

                    yield return new ParseResult(CreateNode(newNodes), newState.Position - startPosition);

                    stack.Push((newNodes, newState));
                }
            }
        }

        protected virtual SyntaxNode CreateNode(params SyntaxNode[] nodes)
        {
            return new UnnamedNode(nodes);
        }
    }
}