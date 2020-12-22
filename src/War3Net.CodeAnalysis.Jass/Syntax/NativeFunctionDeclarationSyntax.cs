// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class NativeFunctionDeclarationSyntax : SyntaxNode
    {
        private readonly TokenNode? _constant;
        private readonly EmptyNode? _empty;
        private readonly TokenNode _native;
        private readonly FunctionDeclarationSyntax _declr;

        public NativeFunctionDeclarationSyntax(TokenNode constantNode, TokenNode nativeNode, FunctionDeclarationSyntax functionDeclarationNode)
            : base(constantNode, nativeNode, functionDeclarationNode)
        {
            _constant = constantNode ?? throw new ArgumentNullException(nameof(constantNode));
            _native = nativeNode ?? throw new ArgumentNullException(nameof(nativeNode));
            _declr = functionDeclarationNode ?? throw new ArgumentNullException(nameof(functionDeclarationNode));
        }

        public NativeFunctionDeclarationSyntax(EmptyNode emptyNode, TokenNode nativeNode, FunctionDeclarationSyntax functionDeclarationNode)
            : base(emptyNode, nativeNode, functionDeclarationNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
            _native = nativeNode ?? throw new ArgumentNullException(nameof(nativeNode));
            _declr = functionDeclarationNode ?? throw new ArgumentNullException(nameof(functionDeclarationNode));
        }

        public TokenNode? ConstantToken => _constant;

        public TokenNode NativeKeywordToken => _native;

        public FunctionDeclarationSyntax FunctionDeclarationNode => _declr;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[0] is TokenNode constantTokenNode)
                {
                    return new NativeFunctionDeclarationSyntax(constantTokenNode, nodes[1] as TokenNode, nodes[2] as FunctionDeclarationSyntax);
                }
                else
                {
                    return new NativeFunctionDeclarationSyntax(nodes[0] as EmptyNode, nodes[1] as TokenNode, nodes[2] as FunctionDeclarationSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(new OptionalParser(TokenParser.Get(SyntaxTokenType.ConstantKeyword)));
                AddParser(TokenParser.Get(SyntaxTokenType.NativeKeyword));
                AddParser(FunctionDeclarationSyntax.Parser.Get);

                return this;
            }
        }
    }
}