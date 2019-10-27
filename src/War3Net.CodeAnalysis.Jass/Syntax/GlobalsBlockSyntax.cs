// ------------------------------------------------------------------------------
// <copyright file="GlobalsBlockSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class GlobalsBlockSyntax : SyntaxNode
    {
        private readonly TokenNode _globals;
        private readonly LineDelimiterSyntax _eol;
        private readonly GlobalsDeclarationListSyntax _globalsList;
        private readonly TokenNode _endglobals;

        public GlobalsBlockSyntax(TokenNode globalsNode, LineDelimiterSyntax eolNode, GlobalsDeclarationListSyntax globalsListNode, TokenNode endglobalsNode)
            : base(globalsNode, eolNode, globalsListNode, endglobalsNode)
        {
            _globals = globalsNode ?? throw new ArgumentNullException(nameof(globalsNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
            _globalsList = globalsListNode ?? throw new ArgumentNullException(nameof(globalsListNode));
            _endglobals = endglobalsNode ?? throw new ArgumentNullException(nameof(endglobalsNode));
        }

        public TokenNode GlobalsKeywordToken => _globals;

        public LineDelimiterSyntax LineDelimiterNode => _eol;

        public GlobalsDeclarationListSyntax GlobalDeclarationListNode => _globalsList;

        public TokenNode EndglobalsKeywordToken => _endglobals;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new GlobalsBlockSyntax(nodes[0] as TokenNode, nodes[1] as LineDelimiterSyntax, nodes[2] as GlobalsDeclarationListSyntax, nodes[3] as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.GlobalsKeyword));
                AddParser(LineDelimiterSyntax.Parser.Get);
                AddParser(GlobalsDeclarationListSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.EndglobalsKeyword));

                return this;
            }
        }
    }
}