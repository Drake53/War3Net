// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class UnaryExpressionSyntax : SyntaxNode
    {
        private readonly UnaryOperatorSyntax _operator;
        private readonly NewExpressionSyntax _expression;

        public UnaryExpressionSyntax(UnaryOperatorSyntax operatorNode, NewExpressionSyntax expressionNode)
            : base(operatorNode, expressionNode)
        {
            _operator = operatorNode ?? throw new ArgumentNullException(nameof(operatorNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public UnaryOperatorSyntax UnaryOperatorNode => _operator;

        public NewExpressionSyntax ExpressionNode => _expression;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new UnaryExpressionSyntax(nodes[0] as UnaryOperatorSyntax, nodes[1] as NewExpressionSyntax);
            }

            private Parser Init()
            {
                AddParser(UnaryOperatorSyntax.Parser.Get);
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}