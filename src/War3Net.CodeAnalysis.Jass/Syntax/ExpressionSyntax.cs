// ------------------------------------------------------------------------------
// <copyright file="ExpressionSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ExpressionSyntax : SyntaxNode
    {
        private readonly UnaryExpressionSyntax _unaryExpr;
        private readonly FunctionCallSyntax _funcCall;
        private readonly ArrayReferenceSyntax _array;
        private readonly FunctionReferenceSyntax _funcRef;
        private readonly TokenNode _id;
        private readonly ConstantExpressionSyntax _constant;
        private readonly ParenthesizedExpressionSyntax _parensExpr;

        public ExpressionSyntax(UnaryExpressionSyntax unaryExpressionNode)
            : base(unaryExpressionNode)
        {
            _unaryExpr = unaryExpressionNode ?? throw new ArgumentNullException(nameof(unaryExpressionNode));
        }

        public ExpressionSyntax(FunctionCallSyntax functionCallNode)
            : base(functionCallNode)
        {
            _funcCall = functionCallNode ?? throw new ArgumentNullException(nameof(functionCallNode));
        }

        public ExpressionSyntax(ArrayReferenceSyntax arrayReferenceNode)
            : base(arrayReferenceNode)
        {
            _array = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));
        }

        public ExpressionSyntax(FunctionReferenceSyntax functionReferenceNode)
            : base(functionReferenceNode)
        {
            _funcRef = functionReferenceNode ?? throw new ArgumentNullException(nameof(functionReferenceNode));
        }

        public ExpressionSyntax(TokenNode idNode)
            : base(idNode)
        {
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
        }

        public ExpressionSyntax(ConstantExpressionSyntax constantExpressionNode)
            : base(constantExpressionNode)
        {
            _constant = constantExpressionNode ?? throw new ArgumentNullException(nameof(constantExpressionNode));
        }

        public ExpressionSyntax(ParenthesizedExpressionSyntax parenthesizedExpressionNode)
            : base(parenthesizedExpressionNode)
        {
            _parensExpr = parenthesizedExpressionNode ?? throw new ArgumentNullException(nameof(parenthesizedExpressionNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is UnaryExpressionSyntax unaryExpressionNode)
                {
                    return new ExpressionSyntax(unaryExpressionNode);
                }

                if (node is FunctionCallSyntax functionCallNode)
                {
                    return new ExpressionSyntax(functionCallNode);
                }

                if (node is ArrayReferenceSyntax arrayReferenceNode)
                {
                    return new ExpressionSyntax(arrayReferenceNode);
                }

                if (node is FunctionReferenceSyntax functionReferenceNode)
                {
                    return new ExpressionSyntax(functionReferenceNode);
                }

                if (node is TokenNode idNode)
                {
                    return new ExpressionSyntax(idNode);
                }

                if (node is ConstantExpressionSyntax constantExpressionNode)
                {
                    return new ExpressionSyntax(constantExpressionNode);
                }

                if (node is ParenthesizedExpressionSyntax parenthesizedExpressionNode)
                {
                    return new ExpressionSyntax(parenthesizedExpressionNode);
                }

                throw new Exception();
            }

            private Parser Init()
            {
                AddParser(UnaryExpressionSyntax.Parser.Get);
                AddParser(FunctionCallSyntax.Parser.Get);
                AddParser(ArrayReferenceSyntax.Parser.Get);
                AddParser(FunctionReferenceSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(ConstantExpressionSyntax.Parser.Get);
                AddParser(ParenthesizedExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}