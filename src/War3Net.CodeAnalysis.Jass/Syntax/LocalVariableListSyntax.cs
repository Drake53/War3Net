// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListSyntax.cs" company="Drake53">
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
    public sealed class LocalVariableListSyntax : SyntaxNode, IEnumerable<LocalVariableDeclarationSyntax>
    {
        private readonly List<LocalVariableDeclarationSyntax> _locals;
        private readonly EmptyNode _empty;

        public LocalVariableListSyntax(params LocalVariableDeclarationSyntax[] localDeclarationNodes)
            : base(localDeclarationNodes)
        {
            // TODO: check not null
            _locals = new List<LocalVariableDeclarationSyntax>(localDeclarationNodes);
        }

        public LocalVariableListSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        public IEnumerator<LocalVariableDeclarationSyntax> GetEnumerator()
        {
            return (_empty is null
                ? _locals
                : Enumerable.Empty<LocalVariableDeclarationSyntax>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_empty is null
                ? _locals
                : Enumerable.Empty<LocalVariableDeclarationSyntax>())
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
                    return new LocalVariableListSyntax(emptyNode);
                }
                else
                {
                    return new LocalVariableListSyntax(node.GetChildren().Select(n => n as LocalVariableDeclarationSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(LocalVariableDeclarationSyntax.Parser.Get);

                return this;
            }
        }
    }
}