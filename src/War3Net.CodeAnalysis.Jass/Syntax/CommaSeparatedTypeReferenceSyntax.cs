// ------------------------------------------------------------------------------
// <copyright file="CommaSeparatedTypeReferenceSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class CommaSeparatedTypeReferenceSyntax : SyntaxNode
    {
        private readonly TokenNode _comma;
        private readonly TypeReferenceSyntax _type;

        public CommaSeparatedTypeReferenceSyntax(TokenNode commaNode, TypeReferenceSyntax typeReferenceNode)
            : base(commaNode, typeReferenceNode)
        {
            _comma = commaNode ?? throw new ArgumentNullException(nameof(commaNode));
            _type = typeReferenceNode ?? throw new ArgumentNullException(nameof(typeReferenceNode));
        }

        public TokenNode CommaToken => _comma;

        public TypeReferenceSyntax TypeReferenceNode => _type;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new CommaSeparatedTypeReferenceSyntax(nodes[0] as TokenNode, nodes[1] as TypeReferenceSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.Comma));
                AddParser(TypeReferenceSyntax.Parser.Get);

                return this;
            }
        }
    }
}