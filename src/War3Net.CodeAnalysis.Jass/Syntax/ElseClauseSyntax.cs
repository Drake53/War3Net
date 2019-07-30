// ------------------------------------------------------------------------------
// <copyright file="ElseClauseSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ElseClauseSyntax : SyntaxNode
    {
        private readonly ElseifClauseSyntax _elseif;
        private readonly EmptyNode _emptyIf;
        private readonly ElseSyntax _else;
        private readonly EmptyNode _empty;

        public ElseClauseSyntax(ElseifClauseSyntax elseifNode, ElseSyntax elseNode)
            : base(elseifNode, elseNode)
        {
            _elseif = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));
            _else = elseNode ?? throw new ArgumentNullException(nameof(elseNode));
        }

        public ElseClauseSyntax(EmptyNode emptyElseifNode, ElseSyntax elseNode)
            : base(emptyElseifNode, elseNode)
        {
            _emptyIf = emptyElseifNode ?? throw new ArgumentNullException(nameof(emptyElseifNode));
            _else = elseNode ?? throw new ArgumentNullException(nameof(elseNode));
        }

        public ElseClauseSyntax(ElseifClauseSyntax elseifNode, EmptyNode emptyElseNode)
            : base(elseifNode, emptyElseNode)
        {
            _elseif = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));
            _empty = emptyElseNode ?? throw new ArgumentNullException(nameof(emptyElseNode));
        }

        public ElseClauseSyntax(EmptyNode emptyElseifNode, EmptyNode emptyElseNode)
            : base(emptyElseifNode, emptyElseNode)
        {
            _emptyIf = emptyElseifNode ?? throw new ArgumentNullException(nameof(emptyElseifNode));
            _empty = emptyElseNode ?? throw new ArgumentNullException(nameof(emptyElseNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[0] is EmptyNode emptyIf)
                {
                    if (nodes[1] is EmptyNode emptyNode)
                    {
                        return new ElseClauseSyntax(emptyIf, emptyNode);
                    }
                    else
                    {
                        return new ElseClauseSyntax(emptyIf, nodes[1] as ElseSyntax);
                    }
                }
                else
                {
                    if (nodes[1] is EmptyNode emptyNode)
                    {
                        return new ElseClauseSyntax(nodes[0] as ElseifClauseSyntax, emptyNode);
                    }
                    else
                    {
                        return new ElseClauseSyntax(nodes[0] as ElseifClauseSyntax, nodes[1] as ElseSyntax);
                    }
                }
            }

            private Parser Init()
            {
                AddParser(new OptionalParser(ElseifClauseSyntax.Parser.Get));
                AddParser(new OptionalParser(ElseSyntax.Parser.Get));

                return this;
            }
        }
    }
}