// ------------------------------------------------------------------------------
// <copyright file="CommaSeparatedExpressionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class CommaSeparatedExpressionSyntax : SyntaxNode
    {
        private readonly TokenNode _comma;
        private readonly NewExpressionSyntax _expression;

        public CommaSeparatedExpressionSyntax(TokenNode commaNode, NewExpressionSyntax expressionNode)
            : base(commaNode, expressionNode)
        {
            _comma = commaNode ?? throw new ArgumentNullException(nameof(commaNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public TokenNode CommaToken => _comma;

        public NewExpressionSyntax ExpressionNode => _expression;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new CommaSeparatedExpressionSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.Comma));
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}