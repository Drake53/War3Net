// ------------------------------------------------------------------------------
// <copyright file="ExitStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ExitStatementSyntax : SyntaxNode
    {
        private readonly TokenNode _exit;
        private readonly NewExpressionSyntax _expression;

        public ExitStatementSyntax(TokenNode exitNode, NewExpressionSyntax expressionNode)
            : base(exitNode, expressionNode)
        {
            _exit = exitNode ?? throw new ArgumentNullException(nameof(exitNode));
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(exitNode));
        }

        public TokenNode ExitwhenKeywordToken => _exit;

        public NewExpressionSyntax ConditionExpressionNode => _expression;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ExitStatementSyntax(nodes[0] as TokenNode, nodes[1] as NewExpressionSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ExitwhenKeyword));
                AddParser(NewExpressionSyntax.Parser.Get);

                return this;
            }
        }
    }
}