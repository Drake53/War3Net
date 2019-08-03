// ------------------------------------------------------------------------------
// <copyright file="LoopStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class LoopStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _loop;
        private readonly LineDelimiterSyntax _eol;
        private readonly StatementListSyntax _statements;
        private readonly TokenNode _endloop;

        public LoopStatementSyntax(TokenNode loopNode, LineDelimiterSyntax eolNode, StatementListSyntax statementListNode, TokenNode endloopNode)
            : base(loopNode, eolNode, statementListNode, endloopNode)
        {
            _loop = loopNode ?? throw new ArgumentNullException(nameof(loopNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));
            _endloop = endloopNode ?? throw new ArgumentNullException(nameof(endloopNode));
        }

        public TokenNode LoopKeywordToken => _loop;

        public StatementListSyntax StatementListNode => _statements;

        public TokenNode EndloopKeywordToken => _endloop;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new LoopStatementSyntax(nodes[0] as TokenNode, nodes[1] as LineDelimiterSyntax, nodes[2] as StatementListSyntax, nodes[3] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.LoopKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(StatementListSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.EndloopKeyword));

                return this;
            }
        }
    }
}