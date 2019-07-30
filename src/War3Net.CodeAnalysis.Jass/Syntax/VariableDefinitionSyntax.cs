// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class VariableDefinitionSyntax : SyntaxNode
    {
        private readonly TypeSyntax _type;
        private readonly TokenNode _id;
        private readonly AssignmentExpressionSyntax _assExpr;
        private readonly EmptyNode _empty;

        public VariableDefinitionSyntax(TypeSyntax typeNode, TokenNode idNode, AssignmentExpressionSyntax assignmentExpressionNode)
            : base(typeNode, idNode, assignmentExpressionNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _assExpr = assignmentExpressionNode ?? throw new ArgumentNullException(nameof(assignmentExpressionNode));
        }

        public VariableDefinitionSyntax(TypeSyntax typeNode, TokenNode idNode, EmptyNode emptyNode)
            : base(typeNode, idNode, emptyNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[2] is EmptyNode emptyNode)
                {
                    return new VariableDefinitionSyntax(nodes[0] as TypeSyntax, nodes[1] as TokenNode, emptyNode);
                }
                else
                {
                    return new VariableDefinitionSyntax(nodes[0] as TypeSyntax, nodes[1] as TokenNode, nodes[2] as AssignmentExpressionSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(TypeSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(new OptionalParser(AssignmentExpressionSyntax.Parser.Get));

                return this;
            }
        }
    }
}