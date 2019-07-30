// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class LineDelimiterSyntax : SyntaxNode
    {
        private readonly List<EndOfLineSyntax> _lines;

        public LineDelimiterSyntax(params EndOfLineSyntax[] nodes)
            : base(nodes)
        {
            // TODO: check not null
            _lines = new List<EndOfLineSyntax>(nodes);
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