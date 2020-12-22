// ------------------------------------------------------------------------------
// <copyright file="ParameterListTailSyntax.cs" company="Drake53">
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
    public sealed class ParameterListTailSyntax : SyntaxNode, IEnumerable<TypeReferenceSyntax>
    {
        private readonly List<CommaSeparatedTypeReferenceSyntax>? _types;
        private readonly EmptyNode? _empty;

        public ParameterListTailSyntax(params CommaSeparatedTypeReferenceSyntax[] typeReferenceNodes)
            : base(typeReferenceNodes)
        {
            // TODO: check not null
            _types = new List<CommaSeparatedTypeReferenceSyntax>(typeReferenceNodes);
        }

        public ParameterListTailSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        public CommaSeparatedTypeReferenceSyntax[] Parameters => _types?.ToArray() ?? Array.Empty<CommaSeparatedTypeReferenceSyntax>();

        public IEnumerator<TypeReferenceSyntax> GetEnumerator()
        {
            if (_types is not null)
            {
                return _types.Select(node => node.TypeReferenceNode).GetEnumerator();
            }

            return Enumerable.Empty<TypeReferenceSyntax>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_types is not null)
            {
                return _types.Select(node => node.TypeReferenceNode).GetEnumerator();
            }

            return Enumerable.Empty<TypeReferenceSyntax>().GetEnumerator();
        }

        internal sealed class Parser : ManyParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is EmptyNode emptyNode)
                {
                    return new ParameterListTailSyntax(emptyNode);
                }
                else
                {
                    return new ParameterListTailSyntax(node.GetChildren().Select(n => n as CommaSeparatedTypeReferenceSyntax).ToArray());
                }
            }

            private Parser Init()
            {
                SetParser(CommaSeparatedTypeReferenceSyntax.Parser.Get);

                return this;
            }
        }
    }
}