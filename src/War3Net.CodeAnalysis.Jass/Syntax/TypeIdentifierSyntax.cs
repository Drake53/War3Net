// ------------------------------------------------------------------------------
// <copyright file="TypeIdentifierSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class TypeIdentifierSyntax : SyntaxNode
    {
        private readonly TokenNode? _nothing;
        private readonly TypeSyntax? _type;

        public TypeIdentifierSyntax(TokenNode nothingNode)
            : base(nothingNode)
        {
            _nothing = nothingNode ?? throw new ArgumentNullException(nameof(nothingNode));
        }

        public TypeIdentifierSyntax(TypeSyntax typeNode)
            : base(typeNode)
        {
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
        }

        public TokenNode? NothingKeywordToken => _nothing;

        public TypeSyntax? TypeNameNode => _type;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is TypeSyntax type)
                {
                    return new TypeIdentifierSyntax(type);
                }
                else
                {
                    return new TypeIdentifierSyntax(node as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.NothingKeyword));
                AddParser(TypeSyntax.Parser.Get);

                return this;
            }
        }
    }
}