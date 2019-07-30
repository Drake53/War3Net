// ------------------------------------------------------------------------------
// <copyright file="ParameterListReferenceSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ParameterListReferenceSyntax : SyntaxNode
    {
        private readonly TokenNode _nothing;
        private readonly ParameterListSyntax _params;

        public ParameterListReferenceSyntax(TokenNode nothingNode)
            : base(nothingNode)
        {
            _nothing = nothingNode ?? throw new ArgumentNullException(nameof(nothingNode));
        }

        public ParameterListReferenceSyntax(ParameterListSyntax parameterListNode)
            : base(parameterListNode)
        {
            _params = parameterListNode ?? throw new ArgumentNullException(nameof(parameterListNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is ParameterListSyntax parameterListNode)
                {
                    return new ParameterListReferenceSyntax(parameterListNode);
                }
                else
                {
                    return new ParameterListReferenceSyntax(node as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.NothingKeyword));
                AddParser(ParameterListSyntax.Parser.Get);

                return this;
            }
        }
    }
}