// ------------------------------------------------------------------------------
// <copyright file="TypeSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class TypeSyntax : SyntaxNode
    {
        private readonly TokenNode _type;

        public TypeSyntax(TokenNode typeNode)
            : base(typeNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new TypeSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.CodeKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.HandleKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.IntegerKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.RealKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.BooleanKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.StringKeyword));

                return this;
            }
        }
    }
}