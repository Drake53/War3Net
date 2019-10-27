// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class LineDelimiterSyntax : SyntaxNode, IEnumerable<EndOfLineSyntax>
    {
        private readonly List<EndOfLineSyntax> _lines;

        public LineDelimiterSyntax(params EndOfLineSyntax[] nodes)
            : base(nodes)
        {
            // TODO: check not null
            _lines = new List<EndOfLineSyntax>(nodes);
        }

        public IEnumerator<EndOfLineSyntax> GetEnumerator()
        {
            return ((IEnumerable<EndOfLineSyntax>)_lines).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<EndOfLineSyntax>)_lines).GetEnumerator();
        }

        internal sealed class Parser : Many1Parser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(params SyntaxNode[] nodes)
            {
                return new LineDelimiterSyntax(nodes.Select(n => n as EndOfLineSyntax).ToArray());
            }

            private Parser Init()
            {
                SetParser(EndOfLineSyntax.Parser.Get);

                return this;
            }
        }
    }
}