// ------------------------------------------------------------------------------
// <copyright file="StringSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class StringSyntax : SyntaxNode
    {
        private readonly TokenNode _open;
        private readonly TokenNode _string;
        private readonly EmptyNode _empty;
        private readonly TokenNode _close;

        public StringSyntax(TokenNode openNode, TokenNode stringNode, TokenNode closeNode)
            : base(openNode, stringNode, closeNode)
        {
            _open = openNode ?? throw new ArgumentNullException(nameof(openNode));
            _string = stringNode ?? throw new ArgumentNullException(nameof(stringNode));
            _close = closeNode ?? throw new ArgumentNullException(nameof(closeNode));
        }

        public StringSyntax(TokenNode openNode, EmptyNode emptyStringNode, TokenNode closeNode)
            : base(openNode, emptyStringNode, closeNode)
        {
            _open = openNode ?? throw new ArgumentNullException(nameof(openNode));
            _empty = emptyStringNode ?? throw new ArgumentNullException(nameof(emptyStringNode));
            _close = closeNode ?? throw new ArgumentNullException(nameof(closeNode));
        }

        public TokenNode StartStringSymbol => _open;

        public TokenNode StringNode => _string;

        public TokenNode EndStringSymbol => _close;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[1] is EmptyNode emptyNode)
                {
                    return new StringSyntax(nodes[0] as TokenNode, emptyNode, nodes[2] as TokenNode);
                }
                else
                {
                    return new StringSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.DoubleQuotes));
                AddParser(new OptionalParser(TokenParser.Get(SyntaxTokenType.String)));
                AddParser(TokenParser.Get(SyntaxTokenType.DoubleQuotes));

                return this;
            }
        }
    }
}