// ------------------------------------------------------------------------------
// <copyright file="ParameterListSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ParameterListSyntax : SyntaxNode
    {
        private readonly TypeReferenceSyntax _head;
        private readonly ParameterListTailSyntax _tail;

        public ParameterListSyntax(TypeReferenceSyntax headNode, ParameterListTailSyntax tailNode)
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
                return new ParameterListSyntax(nodes[0] as TypeReferenceSyntax, nodes[1] as ParameterListTailSyntax);
            }

            private Parser Init()
            {
                AddParser(TypeReferenceSyntax.Parser.Get);
                AddParser(ParameterListTailSyntax.Parser.Get);

                return this;
            }
        }
    }
}