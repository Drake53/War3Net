// ------------------------------------------------------------------------------
// <copyright file="TypeDefinitionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class TypeDefinitionSyntax : SyntaxNode
    {
        private readonly TokenNode _type;
        private readonly TokenNode _sub;
        private readonly TokenNode _extends;
        private readonly ExtendableTypeReferenceSyntax _base;

        public TypeDefinitionSyntax(TokenNode typeNode, TokenNode subTypeNode, TokenNode extendsNode, ExtendableTypeReferenceSyntax baseTypeNode)
            : base(typeNode, subTypeNode, extendsNode, baseTypeNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _sub = subTypeNode ?? throw new ArgumentNullException(nameof(subTypeNode));
            _extends = extendsNode ?? throw new ArgumentNullException(nameof(extendsNode));
            _base = baseTypeNode ?? throw new ArgumentNullException(nameof(baseTypeNode));
        }

        public TokenNode TypeKeywordNode => _type;

        public TokenNode NewTypeNameNode => _sub;

        public TokenNode ExtendsKeywordNode => _extends;

        public ExtendableTypeReferenceSyntax BaseTypeNode => _base;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new TypeDefinitionSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode, nodes[3] as ExtendableTypeReferenceSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.TypeKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.ExtendsKeyword));
                AddParser(ExtendableTypeReferenceSyntax.Parser.Get);

                return this;
            }
        }
    }
}