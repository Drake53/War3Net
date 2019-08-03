// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class UnaryOperatorSyntax : SyntaxNode
    {
        private readonly TokenNode _operator;

        public UnaryOperatorSyntax(TokenNode operatorNode)
            : base(operatorNode)
        {
            _operator = operatorNode ?? throw new ArgumentNullException(nameof(operatorNode));
        }

        public TokenNode UnaryOperatorToken => _operator;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new UnaryOperatorSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.PlusOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.MinusOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.NotOperator));

                return this;
            }
        }
    }
}