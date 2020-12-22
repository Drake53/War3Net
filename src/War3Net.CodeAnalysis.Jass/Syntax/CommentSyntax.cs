// ------------------------------------------------------------------------------
// <copyright file="CommentSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class CommentSyntax : SyntaxNode
    {
        private readonly TokenNode _slashes;
        private readonly TokenNode? _comment;
        private readonly EmptyNode? _emptyComment;
        private readonly TokenNode _newline;

        public CommentSyntax(TokenNode slashesNode, TokenNode commentNode, TokenNode newlineNode)
            : base(slashesNode, commentNode, newlineNode)
        {
            _slashes = slashesNode ?? throw new ArgumentNullException(nameof(slashesNode));
            _comment = commentNode ?? throw new ArgumentNullException(nameof(commentNode));
            _newline = newlineNode ?? throw new ArgumentNullException(nameof(newlineNode));
        }

        public CommentSyntax(TokenNode slashesNode, EmptyNode emptyCommentNode, TokenNode newlineNode)
            : base(slashesNode, emptyCommentNode, newlineNode)
        {
            _slashes = slashesNode ?? throw new ArgumentNullException(nameof(slashesNode));
            _emptyComment = emptyCommentNode ?? throw new ArgumentNullException(nameof(emptyCommentNode));
            _newline = newlineNode ?? throw new ArgumentNullException(nameof(newlineNode));
        }

        public TokenNode DoubleForwardSlashToken => _slashes;

        public TokenNode? CommentNode => _comment;

        public TokenNode NewlineToken => _newline;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                if (nodes[1] is EmptyNode emptyNode)
                {
                    return new CommentSyntax(nodes[0] as TokenNode, emptyNode, nodes[2] as TokenNode);
                }
                else
                {
                    return new CommentSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.DoubleForwardSlash));
                AddParser(new OptionalParser(TokenParser.Get(SyntaxTokenType.Comment)));
                AddParser(TokenParser.Get(SyntaxTokenType.NewlineSymbol));

                return this;
            }
        }
    }
}