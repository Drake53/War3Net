// ------------------------------------------------------------------------------
// <copyright file="ReturnStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ReturnStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _return;
        private readonly NewExpressionSyntax _expression;
        private readonly EmptyNode _empty;

        public ReturnStatementSyntax(TokenNode returnNode, NewExpressionSyntax expressionNode)
            : base(returnNode, expressionNode)
        {
            _return = returnNode ?? throw new ArgumentNullException(nameof(returnNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public ReturnStatementSyntax(TokenNode returnNode, EmptyNode emptyNode)
            : base(returnNode, emptyNode)
        {
            _return = returnNode ?? throw new ArgumentNullException(nameof(returnNode));
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[1] is NewExpressionSyntax expressionNode)
                {
                    return new ReturnStatementSyntax(nodes[0] as TokenNode, expressionNode);
                }
                else
                {
                    return new ReturnStatementSyntax(nodes[0] as TokenNode, nodes[1] as EmptyNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ReturnKeyword));
                AddParser(new OptionalParser(NewExpressionSyntax.Parser.Get));

                return this;
            }
        }
    }
}