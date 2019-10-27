// ------------------------------------------------------------------------------
// <copyright file="ParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ParameterListSyntax : SyntaxNode, IEnumerable<TypeReferenceSyntax>
    {
        private readonly TypeReferenceSyntax _head;
        private readonly ParameterListTailSyntax _tail;

        public ParameterListSyntax(TypeReferenceSyntax headNode, ParameterListTailSyntax tailNode)
            : base(headNode, tailNode)
        {
            _head = headNode ?? throw new ArgumentNullException(nameof(headNode));
            _tail = tailNode ?? throw new ArgumentNullException(nameof(tailNode));
        }

        public TypeReferenceSyntax FirstParameter => _head;

        public ParameterListTailSyntax RemainingParameters => _tail;

        public IEnumerator<TypeReferenceSyntax> GetEnumerator()
        {
            yield return _head;

            foreach (var node in _tail)
            {
                yield return node;
            }

            // return ((IEnumerable<TypeReferenceSyntax>)_tail).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _head;

            foreach (var node in _tail)
            {
                yield return node;
            }

            // return ((IEnumerable<TypeReferenceSyntax>)_tail).GetEnumerator();
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