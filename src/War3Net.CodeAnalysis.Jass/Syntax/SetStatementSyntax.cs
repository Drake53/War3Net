// ------------------------------------------------------------------------------
// <copyright file="SetStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
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
        private readonly EqualsValueClauseSyntax _assExpr;

        public SetStatementSyntax(TokenNode setNode, TokenNode idNode, BracketedExpressionSyntax arrayIndexerNode, EqualsValueClauseSyntax equalsValueClauseNode)
            : base(setNode, idNode, arrayIndexerNode, equalsValueClauseNode)
        {
            _set = setNode ?? throw new ArgumentNullException(nameof(setNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _indexer = arrayIndexerNode ?? throw new ArgumentNullException(nameof(arrayIndexerNode));
            _assExpr = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));
        }

        public SetStatementSyntax(TokenNode setNode, TokenNode idNode, EmptyNode emptyArrayIndexerNode, EqualsValueClauseSyntax equalsValueClauseNode)
            : base(setNode, idNode, emptyArrayIndexerNode, equalsValueClauseNode)
        {
            _set = setNode ?? throw new ArgumentNullException(nameof(setNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _empty = emptyArrayIndexerNode ?? throw new ArgumentNullException(nameof(emptyArrayIndexerNode));
            _assExpr = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));
        }

        public TokenNode SetKeywordToken => _set;

        public TokenNode IdentifierNameNode => _id;

        public BracketedExpressionSyntax ArrayIndexerNode => _indexer;

        public EmptyNode EmptyArrayIndexerNode => _empty;

        public EqualsValueClauseSyntax EqualsValueClauseNode => _assExpr;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[2] is BracketedExpressionSyntax arrayIndexerNode)
                {
                    return new SetStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, arrayIndexerNode, nodes[3] as EqualsValueClauseSyntax);
                }
                else
                {
                    return new SetStatementSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as EmptyNode, nodes[3] as EqualsValueClauseSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.SetKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(new OptionalParser(BracketedExpressionSyntax.Parser.Get));
                AddParser(EqualsValueClauseSyntax.Parser.Get);

                return this;
            }
        }
    }
}