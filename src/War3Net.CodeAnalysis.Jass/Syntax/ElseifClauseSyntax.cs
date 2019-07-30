// ------------------------------------------------------------------------------
// <copyright file="ElseifClauseSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ElseifClauseSyntax : SyntaxNode
    {
        private readonly List<ElseifSyntax> _elseifs;

        public ElseifClauseSyntax(params ElseifSyntax[] nodes)
            : base(nodes)
        {
            // TODO: check not null
            _elseifs = new List<ElseifSyntax>(nodes);
        }

        internal sealed class Parser : Many1Parser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(params SyntaxNode[] nodes)
            {
                return new ElseifClauseSyntax(nodes.Select(n => n as ElseifSyntax).ToArray());
            }

            private Parser Init()
            {
                SetParser(ElseifSyntax.Parser.Get);

                return this;
            }
        }
    }
}