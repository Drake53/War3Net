// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class EqualsValueClauseSyntax : SyntaxNode
    {
        private readonly TokenNode _ass;
        private readonly NewExpressionSyntax _expression;

        public EqualsValueClauseSyntax(TokenNode assignmentNode, NewExpressionSyntax expressionNode)
            : base(assignmentNode, expressionNode)
        {
            _ass = assignmentNode ?? throw new ArgumentNullException(nameof(assignmentNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public TokenNode AssignmentSymbolToken => _ass;

        public NewExpressionSyntax ValueNode => _expression;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new EqualsValueClauseSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.Assignment));
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}