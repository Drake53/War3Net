// ------------------------------------------------------------------------------
// <copyright file="ElseifSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ElseifSyntax : SyntaxNode
    {
        private readonly TokenNode _elseif;
        private readonly NewExpressionSyntax _expression;
        private readonly TokenNode _then;
        private readonly LineDelimiterSyntax _eol;
        private readonly StatementListSyntax _statements;
        private readonly ElseClauseSyntax _elseClause;
        private readonly EmptyNode _emptyElseClause;

        public ElseifSyntax(TokenNode elseifNode, NewExpressionSyntax expressionNode, TokenNode thenNode, LineDelimiterSyntax eolNode, StatementListSyntax statementsNode, ElseClauseSyntax elseClauseNode)
            : base(elseifNode, expressionNode, thenNode, eolNode, statementsNode, elseClauseNode)
        {
            _elseif = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _then = thenNode ?? throw new ArgumentNullException(nameof(thenNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementsNode ?? throw new ArgumentNullException(nameof(statementsNode));
            _elseClause = elseClauseNode ?? throw new ArgumentNullException(nameof(elseClauseNode));
        }

        public ElseifSyntax(TokenNode elseifNode, NewExpressionSyntax expressionNode, TokenNode thenNode, LineDelimiterSyntax eolNode, StatementListSyntax statementsNode, EmptyNode emptyElseClauseNode)
            : base(elseifNode, expressionNode, thenNode, eolNode, statementsNode, emptyElseClauseNode)
        {
            _elseif = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
            _then = thenNode ?? throw new ArgumentNullException(nameof(thenNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementsNode ?? throw new ArgumentNullException(nameof(statementsNode));
            _emptyElseClause = emptyElseClauseNode ?? throw new ArgumentNullException(nameof(emptyElseClauseNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[5] is EmptyNode emptyNode)
                {
                    return new ElseifSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax, nodes[2] as TokenNode, nodes[3] as LineDelimiterSyntax, nodes[4] as StatementListSyntax, emptyNode);
                }
                else
                {
                    return new ElseifSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax, nodes[2] as TokenNode, nodes[3] as LineDelimiterSyntax, nodes[4] as StatementListSyntax, nodes[5] as ElseClauseSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ElseifKeyword));
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.ThenKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(StatementListSyntax.Parser.Get);
                AddParser(new OptionalParser(ElseClauseSyntax.Parser.Get));

                return this;
            }
        }
    }
}