// ------------------------------------------------------------------------------
// <copyright file="SetStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class SetStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _set;
        private readonly TokenNode _id;
        private readonly BracketedExpressionSyntax _indexer;
        private readonly EmptyNode _empty;
        private readonly TokenNode _ass;
        private readonly NewExpressionSyntax _expression;

        public SetStatementSyntax(TokenNode setNode, TokenNode idNode, BracketedExpressionSyntax arrayIndexerNode, TokenNode assignmentNode, NewExpressionSyntax expressionNode)
            : base(setNode, idNode, arrayIndexerNode, assignmentNode, expressionNode)
        {
            _set = setNode ?? throw new ArgumentNullException(nameof(setNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _indexer = arrayIndexerNode ?? throw new ArgumentNullException(nameof(arrayIndexerNode));
            _ass = assignmentNode ?? throw new ArgumentNullException(nameof(assignmentNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public SetStatementSyntax(TokenNode setNode, TokenNode idNode, EmptyNode emptyArrayIndexerNode, TokenNode assignmentNode, NewExpressionSyntax expressionNode)
            : base(setNode, idNode, emptyArrayIndexerNode, assignmentNode, expressionNode)
        {
            _set = setNode ?? throw new ArgumentNullException(nameof(setNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _empty = emptyArrayIndexerNode ?? throw new ArgumentNullException(nameof(emptyArrayIndexerNode));
            _ass = assignmentNode ?? throw new ArgumentNullException(nameof(assignmentNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[2] is BracketedExpressionSyntax arrayIndexerNode)
                {
                    return new SetStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, arrayIndexerNode, nodes[3] as TokenNode, nodes[4] as NewExpressionSyntax);
                }
                else
                {
                    return new SetStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as EmptyNode, nodes[3] as TokenNode, nodes[4] as NewExpressionSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.SetKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(new OptionalParser(BracketedExpressionSyntax.Parser.Get));
                AddParser(TokenParser.Get(SyntaxTokenType.Assignment));
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}