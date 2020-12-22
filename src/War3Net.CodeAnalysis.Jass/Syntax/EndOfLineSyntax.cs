// ------------------------------------------------------------------------------
// <copyright file="EndOfLineSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class EndOfLineSyntax : SyntaxNode
    {
        private readonly CommentSyntax? _comment;
        private readonly TokenNode? _newline;

        public EndOfLineSyntax(CommentSyntax commentNode)
            : base(commentNode)
        {
            _comment = commentNode ?? throw new ArgumentNullException(nameof(commentNode));
        }

        public EndOfLineSyntax(TokenNode newlineNode)
            : base(newlineNode)
        {
            _newline = newlineNode ?? throw new ArgumentNullException(nameof(newlineNode));
        }

        public CommentSyntax? Comment => _comment;

        public TokenNode? NewlineToken => _newline;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is CommentSyntax commentNode)
                {
                    return new EndOfLineSyntax(commentNode);
                }
                else
                {
                    return new EndOfLineSyntax(node as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(CommentSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.NewlineSymbol));

                return this;
            }
        }
    }
}