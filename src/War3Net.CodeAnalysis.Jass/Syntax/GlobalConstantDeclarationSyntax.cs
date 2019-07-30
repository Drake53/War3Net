// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class GlobalConstantDeclarationSyntax : SyntaxNode
    {
        private readonly TokenNode _constant;
        private readonly TypeSyntax _type;
        private readonly TokenNode _id;
        private readonly TokenNode _ass;
        private readonly NewExpressionSyntax _expression;
        private readonly LineDelimiterSyntax _eol;

        public GlobalConstantDeclarationSyntax(TokenNode constantNode, TypeSyntax typeNode, TokenNode idNode, TokenNode assignmentNode, NewExpressionSyntax expressionNode, LineDelimiterSyntax eolNode)
            : base(constantNode, typeNode, idNode, assignmentNode, expressionNode, eolNode)
        {
            _constant = constantNode ?? throw new ArgumentNullException(nameof(constantNode));
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _ass = assignmentNode ?? throw new ArgumentNullException(nameof(assignmentNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new GlobalConstantDeclarationSyntax(nodes[0] as TokenNode, nodes[1] as TypeSyntax, nodes[2] as TokenNode, nodes[3] as TokenNode, nodes[4] as NewExpressionSyntax, nodes[5] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ConstantKeyword));
                AddParser(TypeSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.Assignment));
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}