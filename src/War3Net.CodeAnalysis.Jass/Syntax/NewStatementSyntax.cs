// ------------------------------------------------------------------------------
// <copyright file="NewStatementSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class NewStatementSyntax : SyntaxNode
    {
        private readonly StatementSyntax _statement;
        private readonly LineDelimiterSyntax _eol;

        public NewStatementSyntax(StatementSyntax statementNode, LineDelimiterSyntax eolNode)
            : base(statementNode, eolNode)
        {
            _statement = statementNode ?? throw new ArgumentNullException(nameof(statementNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new NewStatementSyntax(nodes[0] as StatementSyntax, nodes[1] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(StatementSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}