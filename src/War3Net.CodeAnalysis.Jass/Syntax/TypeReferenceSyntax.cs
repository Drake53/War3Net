// ------------------------------------------------------------------------------
// <copyright file="TypeReferenceSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class TypeReferenceSyntax : SyntaxNode
    {
        private readonly TypeSyntax _type;
        private readonly TokenNode _id;

        public TypeReferenceSyntax(TypeSyntax typeNode, TokenNode idNode)
            : base(typeNode, idNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
        }

        public TypeSyntax TypeNameNode => _type;

        public TokenNode TypeReferenceNameToken => _id;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new TypeReferenceSyntax(nodes[0] as TypeSyntax, nodes[1] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TypeSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));

                return this;
            }
        }
    }
}