// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ArrayReferenceSyntax : SyntaxNode
    {
        private readonly TokenNode _id;
        private readonly TokenNode _open;
        private readonly NewExpressionSyntax _expression;
        private readonly TokenNode _close;

        public ArrayReferenceSyntax(TokenNode idNode, TokenNode openBracketNode, NewExpressionSyntax expressionNode, TokenNode closeBracketNode)
            : base(idNode, openBracketNode, expressionNode, closeBracketNode)
        {
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _open = openBracketNode ?? throw new ArgumentNullException(nameof(openBracketNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _close = closeBracketNode ?? throw new ArgumentNullException(nameof(closeBracketNode));
        }

        public TokenNode IdentifierNameNode => _id;

        public TokenNode OpenBracketSymbol => _open;

        public NewExpressionSyntax IndexExpressionNode => _expression;

        public TokenNode CloseBracketSymbol => _close;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ArrayReferenceSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as NewExpressionSyntax, nodes[3] as TokenNode);
            }

            private Parser Init()
            {
                // TODO: use BracketedExpressionSyntax?
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(TokenParser.Get(SyntaxTokenType.SquareBracketOpenSymbol));
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.SquareBracketCloseSymbol));

                return this;
            }
        }
    }
}