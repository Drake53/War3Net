// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ConstantExpressionSyntax : SyntaxNode
    {
        private readonly IntegerSyntax _integer;
        private readonly TokenNode _real;
        private readonly BooleanSyntax _boolean;
        private readonly StringSyntax _string;
        private readonly TokenNode _null;

        public ConstantExpressionSyntax(IntegerSyntax integerNode)
            : base(integerNode)
        {
            _integer = integerNode ?? throw new ArgumentNullException(nameof(integerNode));
        }

        public ConstantExpressionSyntax(BooleanSyntax booleanNode)
            : base(booleanNode)
        {
            _boolean = booleanNode ?? throw new ArgumentNullException(nameof(booleanNode));
        }

        public ConstantExpressionSyntax(StringSyntax stringNode)
            : base(stringNode)
        {
            _string = stringNode ?? throw new ArgumentNullException(nameof(stringNode));
        }

        public ConstantExpressionSyntax(TokenNode tokenNode)
            : base(tokenNode)
        {
            if ((tokenNode?.TokenType ?? throw new ArgumentNullException(nameof(tokenNode))) == SyntaxTokenType.RealNumber)
            {
                _real = tokenNode;
            }
            else
            {
                _null = tokenNode;
            }
        }

        public IntegerSyntax IntegerExpressionNode => _integer;

        public TokenNode RealExpressionNode => _real;

        public BooleanSyntax BooleanExpressionNode => _boolean;

        public StringSyntax StringExpressionNode => _string;

        public TokenNode NullExpressionNode => _null;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is IntegerSyntax integer)
                {
                    return new ConstantExpressionSyntax(integer);
                }

                if (node is BooleanSyntax boolean)
                {
                    return new ConstantExpressionSyntax(boolean);
                }

                if (node is StringSyntax @string)
                {
                    return new ConstantExpressionSyntax(@string);
                }

                return new ConstantExpressionSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(IntegerSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.RealNumber));
                AddParser(BooleanSyntax.Parser.Get);
                AddParser(StringSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.NullKeyword));

                return this;
            }
        }
    }
}