// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FunctionReferenceSyntax : SyntaxNode
    {
        private readonly TokenNode _function;
        private readonly TokenNode _id;

        public FunctionReferenceSyntax(TokenNode functionNode, TokenNode idNode)
            : base(functionNode, idNode)
        {
            _function = functionNode ?? throw new ArgumentNullException(nameof(functionNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
        }

        public TokenNode IdentifierNameNode => _id;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new FunctionReferenceSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.FunctionKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));

                return this;
            }
        }
    }
}