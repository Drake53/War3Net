// ------------------------------------------------------------------------------
// <copyright file="BracketedExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class BracketedExpressionSyntax : SyntaxNode
    {
        private readonly TokenNode _open;
        private readonly NewExpressionSyntax _expression;
        private readonly TokenNode _close;

        public BracketedExpressionSyntax(TokenNode openNode, NewExpressionSyntax expressionNode, TokenNode closeNode)
            : base(openNode, expressionNode, closeNode)
        {
            _open = openNode ?? throw new ArgumentNullException(nameof(openNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _close = closeNode ?? throw new ArgumentNullException(nameof(closeNode));
        }


        public TokenNode OpenBracketSymbol => _open;

        public NewExpressionSyntax ExpressionNode => _expression;

        public TokenNode CloseBracketSymbol => _close;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new BracketedExpressionSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax, nodes[2] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.SquareBracketOpenSymbol));
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.SquareBracketCloseSymbol));

                return this;
            }
        }
    }
}