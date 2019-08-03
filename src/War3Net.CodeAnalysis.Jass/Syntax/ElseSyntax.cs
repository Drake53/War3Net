// ------------------------------------------------------------------------------
// <copyright file="ElseSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ElseSyntax : SyntaxNode
    {
        private readonly TokenNode _else;
        private readonly LineDelimiterSyntax _eol;
        private readonly StatementListSyntax _statements;

        public ElseSyntax(TokenNode elseNode, LineDelimiterSyntax eolNode, StatementListSyntax statementsNode)
            : base(elseNode, eolNode, statementsNode)
        {
            _else = elseNode ?? throw new ArgumentNullException(nameof(elseNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _statements = statementsNode ?? throw new ArgumentNullException(nameof(statementsNode));
        }

        public TokenNode ElseKeywordToken => _else;

        public StatementListSyntax StatementListNode => _statements;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ElseSyntax(nodes[0] as TokenNode, nodes[1] as LineDelimiterSyntax, nodes[2] as StatementListSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ElseKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(StatementListSyntax.Parser.Get);

                return this;
            }
        }
    }
}