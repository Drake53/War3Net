// ------------------------------------------------------------------------------
// <copyright file="ArrayDefinitionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ArrayDefinitionSyntax : SyntaxNode
    {
        private readonly TypeSyntax _type;
        private readonly TokenNode _array;
        private readonly TokenNode _id;

        public ArrayDefinitionSyntax(TypeSyntax typeNode, TokenNode arrayNode, TokenNode idNode)
            : base(typeNode, arrayNode, idNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _array = arrayNode ?? throw new ArgumentNullException(nameof(arrayNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
        }

        public TypeSyntax TypeNameNode => _type;

        public TokenNode ArrayKeywordToken => _array;

        public TokenNode IdentifierNameNode => _id;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ArrayDefinitionSyntax(nodes[0] as TypeSyntax, nodes[1] as TokenNode, nodes[2] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TypeSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.ArrayKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));

                return this;
            }
        }
    }
}