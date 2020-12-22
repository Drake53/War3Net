// ------------------------------------------------------------------------------
// <copyright file="StatementListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class StatementListSyntax : SyntaxNode, IEnumerable<NewStatementSyntax>
    {
        private readonly List<NewStatementSyntax>? _statements;
        private readonly EmptyNode? _empty;

        public StatementListSyntax(params NewStatementSyntax[] nodes)
            : base(nodes)
        {
            // TODO: check not null
            _statements = new List<NewStatementSyntax>(nodes);
        }

        public StatementListSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        public IEnumerator<NewStatementSyntax> GetEnumerator()
        {
            return (_statements is not null
                ? _statements
                : Enumerable.Empty<NewStatementSyntax>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_statements is not null
                ? _statements
                : Enumerable.Empty<NewStatementSyntax>())
                .GetEnumerator();
        }

        internal sealed class Parser : ManyParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is EmptyNode emptyNode)
                {
                    return new StatementListSyntax(emptyNode);
                }
                else
                {
                    return new StatementListSyntax(node.GetChildren().Select(n => n as NewStatementSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(NewStatementSyntax.Parser.Get);

                return this;
            }
        }
    }
}