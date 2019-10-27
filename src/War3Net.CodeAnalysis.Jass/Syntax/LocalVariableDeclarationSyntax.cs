// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class LocalVariableDeclarationSyntax : SyntaxNode
    {
        private readonly TokenNode _local;
        private readonly VariableDeclarationSyntax _declr;
        private readonly LineDelimiterSyntax _eol;

        public LocalVariableDeclarationSyntax(TokenNode localNode, VariableDeclarationSyntax variableDeclarationNode, LineDelimiterSyntax eolNode)
            : base(localNode, variableDeclarationNode, eolNode)
        {
            _local = localNode ?? throw new ArgumentNullException(nameof(localNode));
            _declr = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        public TokenNode LocalKeywordToken => _local;

        public VariableDeclarationSyntax VariableDeclarationNode => _declr;

        public LineDelimiterSyntax LineDelimiterNode => _eol;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new LocalVariableDeclarationSyntax(nodes[0] as TokenNode, nodes[1] as VariableDeclarationSyntax, nodes[2] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.LocalKeyword));
                AddParser(VariableDeclarationSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}