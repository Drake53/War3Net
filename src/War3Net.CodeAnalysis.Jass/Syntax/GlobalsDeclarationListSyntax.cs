// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationListSyntax.cs" company="Drake53">
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
    public sealed class GlobalsDeclarationListSyntax : SyntaxNode, IEnumerable<GlobalDeclarationSyntax>
    {
        private readonly List<GlobalDeclarationSyntax>? _globals;
        private readonly EmptyNode? _empty;

        public GlobalsDeclarationListSyntax(params GlobalDeclarationSyntax[] globalDeclarationNodes)
            : base(globalDeclarationNodes)
        {
            // TODO: check not null
            _globals = new List<GlobalDeclarationSyntax>(globalDeclarationNodes);
        }

        public GlobalsDeclarationListSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        public IEnumerator<GlobalDeclarationSyntax> GetEnumerator()
        {
            // TODO: use this format for all ienum syntax classes?
            return (_globals is not null
                ? _globals
                : Enumerable.Empty<GlobalDeclarationSyntax>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_globals is not null
                ? _globals
                : Enumerable.Empty<GlobalDeclarationSyntax>())
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
                    return new GlobalsDeclarationListSyntax(emptyNode);
                }
                else
                {
                    return new GlobalsDeclarationListSyntax(node.GetChildren().Select(n => n as GlobalDeclarationSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(GlobalDeclarationSyntax.Parser.Get);

                return this;
            }
        }
    }
}