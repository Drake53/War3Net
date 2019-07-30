// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class BinaryExpressionSyntax : SyntaxNode
    {
        private readonly BinaryOperatorSyntax _operator;
        private readonly NewExpressionSyntax _expression;

        public BinaryExpressionSyntax(BinaryOperatorSyntax operatorNode, NewExpressionSyntax expressionNode)
            : base(operatorNode, expressionNode)
        {
            _operator = operatorNode ?? throw new ArgumentNullException(nameof(operatorNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new BinaryExpressionSyntax(nodes[0] as BinaryOperatorSyntax, nodes[1] as NewExpressionSyntax);
            }

            private Parser Init()
            {
                AddParser(BinaryOperatorSyntax.Parser.Get);
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}