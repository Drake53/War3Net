// ------------------------------------------------------------------------------
// <copyright file="ExtendableTypeReferenceSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ExtendableTypeReferenceSyntax : SyntaxNode
    {
        private readonly TokenNode _handleId;

        public ExtendableTypeReferenceSyntax(TokenNode handleIdNode)
            : base(handleIdNode)
        {
            _handleId = handleIdNode ?? throw new ArgumentNullException(nameof(handleIdNode));
        }

        public TokenNode HandleIdentifierNode => _handleId;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new ExtendableTypeReferenceSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.HandleKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));

                return this;
            }
        }
    }
}