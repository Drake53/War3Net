// ------------------------------------------------------------------------------
// <copyright file="DeclarationListSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class DeclarationListSyntax : SyntaxNode, IEnumerable<NewDeclarationSyntax>
    {
        private readonly List<NewDeclarationSyntax> _declrs;
        private readonly EmptyNode _empty;

        public DeclarationListSyntax(params NewDeclarationSyntax[] declarationNodes)
            : base(declarationNodes)
        {
            // TODO: check not null
            _declrs = new List<NewDeclarationSyntax>(declarationNodes);
        }

        public DeclarationListSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        public IEnumerator<NewDeclarationSyntax> GetEnumerator()
        {
            return (_empty is null
                ? _declrs
                : Enumerable.Empty<NewDeclarationSyntax>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_empty is null
                ? _declrs
                : Enumerable.Empty<NewDeclarationSyntax>())
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
                    return new DeclarationListSyntax(emptyNode);
                }
                else
                {
                    return new DeclarationListSyntax(node.GetChildren().Select(n => n as NewDeclarationSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(NewDeclarationSyntax.Parser.Get);

                return this;
            }
        }
    }
}