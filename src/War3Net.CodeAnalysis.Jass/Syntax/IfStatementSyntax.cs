// ------------------------------------------------------------------------------
// <copyright file="IfStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class IfStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _if;
        private readonly NewExpressionSyntax _expression;
        private readonly TokenNode _then;
        private readonly LineDelimiterSyntax _eol;
        private readonly StatementListSyntax _statements;
        private readonly ElseClauseSyntax? _elseClause;
        private readonly EmptyNode? _emptyElseClause;
        private readonly TokenNode _endif;

        public IfStatementSyntax(TokenNode ifNode, NewExpressionSyntax expressionNode, TokenNode thenNode, LineDelimiterSyntax eolNode, StatementListSyntax statementListNode, ElseClauseSyntax elseClauseNode, TokenNode endifNode)
            : base(ifNode, expressionNode, thenNode, eolNode, statementListNode, elseClauseNode, endifNode)
        {
            _if = ifNode ?? throw new ArgumentNullException(nameof(ifNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _then = thenNode ?? throw new ArgumentNullException(nameof(thenNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));
            _elseClause = elseClauseNode ?? throw new ArgumentNullException(nameof(elseClauseNode));
            _endif = endifNode ?? throw new ArgumentNullException(nameof(endifNode));
        }

        public IfStatementSyntax(TokenNode ifNode, NewExpressionSyntax expressionNode, TokenNode thenNode, LineDelimiterSyntax eolNode, StatementListSyntax statementListNode, EmptyNode emptyElseClauseNode, TokenNode endifNode)
            : base(ifNode, expressionNode, thenNode, eolNode, statementListNode, emptyElseClauseNode, endifNode)
        {
            _if = ifNode ?? throw new ArgumentNullException(nameof(ifNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _then = thenNode ?? throw new ArgumentNullException(nameof(thenNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));
            _emptyElseClause = emptyElseClauseNode ?? throw new ArgumentNullException(nameof(emptyElseClauseNode));
            _endif = endifNode ?? throw new ArgumentNullException(nameof(endifNode));
        }

        public TokenNode IfKeywordToken => _if;

        public NewExpressionSyntax ConditionExpressionNode => _expression;

        public TokenNode ThenKeywordToken => _then;

        public LineDelimiterSyntax LineDelimiterNode => _eol;

        public StatementListSyntax StatementListNode => _statements;

        public ElseClauseSyntax? ElseClauseNode => _elseClause;

        public TokenNode EndifKeywordToken => _endif;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[5] is EmptyNode emptyNode)
                {
                    return new IfStatementSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax, nodes[2] as TokenNode, nodes[3] as LineDelimiterSyntax, nodes[4] as StatementListSyntax, emptyNode, nodes[6] as TokenNode);
                }
                else
                {
                    return new IfStatementSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax, nodes[2] as TokenNode, nodes[3] as LineDelimiterSyntax, nodes[4] as StatementListSyntax, nodes[5] as ElseClauseSyntax, nodes[6] as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.IfKeyword));
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.ThenKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(StatementListSyntax.Parser.Get);
                AddParser(new OptionalParser(ElseClauseSyntax.Parser.Get));
                AddParser(TokenParser.Get(SyntaxTokenType.EndifKeyword));

                return this;
            }
        }
    }
}