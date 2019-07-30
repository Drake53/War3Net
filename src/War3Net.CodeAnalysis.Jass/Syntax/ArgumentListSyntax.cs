// ------------------------------------------------------------------------------
// <copyright file="ArgumentListSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ArgumentListSyntax : SyntaxNode
    {
        private readonly NewExpressionSyntax _head;
        private readonly ArgumentListTailSyntax _tail;

        public ArgumentListSyntax(NewExpressionSyntax headNode, ArgumentListTailSyntax tailNode)
            : base(headNode, tailNode)
        {
            _head = headNode ?? throw new ArgumentNullException(nameof(headNode));
            _tail = tailNode ?? throw new ArgumentNullException(nameof(tailNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new ArgumentListSyntax(nodes[0] as NewExpressionSyntax, nodes[1] as ArgumentListTailSyntax);
            }

            private Parser Init()
            {
                AddParser(NewExpressionSyntax.Parser.Get);
                AddParser(ArgumentListTailSyntax.Parser.Get);

                return this;
            }
        }
    }
}