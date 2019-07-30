// ------------------------------------------------------------------------------
// <copyright file="CallStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class CallStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _call;
        private readonly TokenNode _id;
        private readonly TokenNode _open;
        private readonly ArgumentListSyntax _args;
        private readonly EmptyNode _emptyArgs;
        private readonly TokenNode _close;

        public CallStatementSyntax(TokenNode callNode, TokenNode idNode, TokenNode openParensNode, ArgumentListSyntax argumentListNode, TokenNode closeParensNode)
            : base(callNode, idNode, openParensNode, argumentListNode, closeParensNode)
        {
            _call = callNode ?? throw new ArgumentNullException(nameof(callNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _open = openParensNode ?? throw new ArgumentNullException(nameof(openParensNode));
            _args = argumentListNode ?? throw new ArgumentNullException(nameof(argumentListNode));
            _close = closeParensNode ?? throw new ArgumentNullException(nameof(closeParensNode));
        }

        public CallStatementSyntax(TokenNode callNode, TokenNode idNode, TokenNode openParensNode, EmptyNode emptyArgumentListNode, TokenNode closeParensNode)
            : base(callNode, idNode, openParensNode, emptyArgumentListNode, closeParensNode)
        {
            _call = callNode ?? throw new ArgumentNullException(nameof(callNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _open = openParensNode ?? throw new ArgumentNullException(nameof(openParensNode));
            _emptyArgs = emptyArgumentListNode ?? throw new ArgumentNullException(nameof(emptyArgumentListNode));
            _close = closeParensNode ?? throw new ArgumentNullException(nameof(closeParensNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[3] is ArgumentListSyntax argumentListNode)
                {
                    return new CallStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode, argumentListNode, nodes[4] as TokenNode);
                }
                else
                {
                    return new CallStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode, nodes[3] as EmptyNode, nodes[4] as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.CallKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.ParenthesisOpenSymbol));
                AddParser(new OptionalParser(ArgumentListSyntax.Parser.Get));
                AddParser(TokenParser.Get(SyntaxTokenType.ParenthesisCloseSymbol));

                return this;
            }
        }
    }
}