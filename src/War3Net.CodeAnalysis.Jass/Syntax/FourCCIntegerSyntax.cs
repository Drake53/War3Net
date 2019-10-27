// ------------------------------------------------------------------------------
// <copyright file="FourCCIntegerSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class FourCCIntegerSyntax : SyntaxNode
    {
        private readonly TokenNode _open;
        private readonly TokenNode _fourcc;
        private readonly TokenNode _close;

        public FourCCIntegerSyntax(TokenNode openNode, TokenNode fourCCNode, TokenNode closeNode)
            : base(openNode, fourCCNode, closeNode)
        {
            _open = openNode ?? throw new ArgumentNullException(nameof(openNode));
            _fourcc = fourCCNode ?? throw new ArgumentNullException(nameof(fourCCNode));
            _close = closeNode ?? throw new ArgumentNullException(nameof(closeNode));
        }

        public TokenNode StartFourCCSymbol => _open;

        public TokenNode FourCCNode => _fourcc;

        public TokenNode EndFourCCSymbol => _close;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new FourCCIntegerSyntax(nodes[0] as TokenNode, nodes[1] as TokenNode, nodes[2] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.SingleQuote));
                AddParser(TokenParser.Get(SyntaxTokenType.FourCCNumber));
                AddParser(TokenParser.Get(SyntaxTokenType.SingleQuote));

                return this;
            }
        }
    }
}