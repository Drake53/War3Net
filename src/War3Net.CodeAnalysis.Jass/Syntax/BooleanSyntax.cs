// ------------------------------------------------------------------------------
// <copyright file="BooleanSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class BooleanSyntax : SyntaxNode
    {
        private readonly TokenNode _token;

        public BooleanSyntax(TokenNode tokenNode)
            : base(tokenNode)
        {
            _token = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));
        }

        public TokenNode BooleanNode => _token;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new BooleanSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.TrueKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.FalseKeyword));

                return this;
            }
        }
    }
}