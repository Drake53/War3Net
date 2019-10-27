// ------------------------------------------------------------------------------
// <copyright file="DebugStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class DebugStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _debug;
        private readonly NewDebugStatementSyntax _statement;

        public DebugStatementSyntax(TokenNode debugNode, NewDebugStatementSyntax statementNode)
            : base(debugNode, statementNode)
        {
            _debug = debugNode ?? throw new ArgumentNullException(nameof(debugNode));
            _statement = statementNode ?? throw new ArgumentNullException(nameof(statementNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new DebugStatementSyntax(nodes[0] as TokenNode, nodes[1] as NewDebugStatementSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.DebugKeyword));
                AddParser(NewDebugStatementSyntax.Parser.Get);

                return this;
            }
        }
    }
}