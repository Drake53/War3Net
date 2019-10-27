// ------------------------------------------------------------------------------
// <copyright file="ParameterListReferenceSyntax.cs" company="Drake53">
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
    public sealed class ParameterListReferenceSyntax : SyntaxNode, IEnumerable<TypeReferenceSyntax>
    {
        private readonly TokenNode _nothing;
        private readonly ParameterListSyntax _params;

        public ParameterListReferenceSyntax(TokenNode nothingNode)
            : base(nothingNode)
        {
            _nothing = nothingNode ?? throw new ArgumentNullException(nameof(nothingNode));
        }

        public ParameterListReferenceSyntax(ParameterListSyntax parameterListNode)
            : base(parameterListNode)
        {
            _params = parameterListNode ?? throw new ArgumentNullException(nameof(parameterListNode));
        }

        public TokenNode NothingKeywordToken => _nothing;

        public ParameterListSyntax ParameterListNode => _params;

        public IEnumerator<TypeReferenceSyntax> GetEnumerator()
        {
            if (_nothing is null)
            {
                return _params.GetEnumerator();
            }

            return Enumerable.Empty<TypeReferenceSyntax>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_nothing is null)
            {
                return _params.GetEnumerator();
            }

            return Enumerable.Empty<TypeReferenceSyntax>().GetEnumerator();
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is ParameterListSyntax parameterListNode)
                {
                    return new ParameterListReferenceSyntax(parameterListNode);
                }
                else
                {
                    return new ParameterListReferenceSyntax(node as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.NothingKeyword));
                AddParser(ParameterListSyntax.Parser.Get);

                return this;
            }
        }
    }
}