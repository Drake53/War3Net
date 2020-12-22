// ------------------------------------------------------------------------------
// <copyright file="FunctionCallSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    // TODO: rename FunctionInvocationSyntax
    public sealed class FunctionCallSyntax : SyntaxNode
    {
        private readonly TokenNode _id;
        private readonly TokenNode _open;
        private readonly ArgumentListSyntax? _args;
        private readonly EmptyNode? _emptyArgs;
        private readonly TokenNode _close;

        public FunctionCallSyntax(TokenNode idNode, TokenNode openParensNode, ArgumentListSyntax argumentListNode, TokenNode closeParensNode)
            : base(idNode, openParensNode, argumentListNode, closeParensNode)
        {
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _open = openParensNode ?? throw new ArgumentNullException(nameof(openParensNode));
            _args = argumentListNode ?? throw new ArgumentNullException(nameof(argumentListNode));
            _close = closeParensNode ?? throw new ArgumentNullException(nameof(closeParensNode));
        }

        public FunctionCallSyntax(TokenNode idNode, TokenNode openParensNode, EmptyNode emptyArgumentListNode, TokenNode closeParensNode)
            : base(idNode, openParensNode, emptyArgumentListNode, closeParensNode)
        {
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _open = openParensNode ?? throw new ArgumentNullException(nameof(openParensNode));
            _emptyArgs = emptyArgumentListNode ?? throw new ArgumentNullException(nameof(emptyArgumentListNode));
            _close = closeParensNode ?? throw new ArgumentNullException(nameof(closeParensNode));
        }

        public TokenNode IdentifierNameNode => _id;

        public TokenNode OpenParenthesisSymbol => _open;

        public ArgumentListSyntax? ArgumentListNode => _args;

        public TokenNode CloseParenthesisSymbol => _close;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[2] is ArgumentListSyntax argumentListNode)
                {
                    return new FunctionCallSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, argumentListNode, nodes[3] as TokenNode);
                }
                else
                {
                    return new FunctionCallSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as EmptyNode, nodes[3] as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.ParenthesisOpenSymbol));
                AddParser(new OptionalParser(ArgumentListSyntax.Parser.Get));
                AddParser(TokenParser.Get(SyntaxTokenType.ParenthesisCloseSymbol));

                return this;
            }
        }
    }
}