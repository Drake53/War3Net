// ------------------------------------------------------------------------------
// <copyright file="FunctionListSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FunctionListSyntax : SyntaxNode
    {
        private readonly List<FunctionSyntax> _funcs;
        private readonly EmptyNode _empty;

        public FunctionListSyntax(params FunctionSyntax[] declarationNodes)
            : base(declarationNodes)
        {
            // TODO: check not null
            _funcs = new List<FunctionSyntax>(declarationNodes);
        }

        public FunctionListSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        internal sealed class Parser : ManyParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is EmptyNode emptyNode)
                {
                    return new FunctionListSyntax(emptyNode);
                }
                else
                {
                    return new FunctionListSyntax(node.GetChildren().Select(n => n as FunctionSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(FunctionSyntax.Parser.Get);

                return this;
            }
        }
    }
}