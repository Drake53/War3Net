// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class GlobalConstantDeclarationSyntax : SyntaxNode
    {
        private readonly TokenNode _constant;
        private readonly TypeSyntax _type;
        private readonly TokenNode _id;
        private readonly EqualsValueClauseSyntax _assExpr;
        private readonly LineDelimiterSyntax _eol;

        public GlobalConstantDeclarationSyntax(TokenNode constantNode, TypeSyntax typeNode, TokenNode idNode, EqualsValueClauseSyntax assignmentExpressionNode, LineDelimiterSyntax eolNode)
            : base(constantNode, typeNode, idNode, assignmentExpressionNode, eolNode)
        {
            _constant = constantNode ?? throw new ArgumentNullException(nameof(constantNode));
            _type = typeNode ?? throw new ArgumentNullException(nameof(typeNode));
            _id = idNode ?? throw new ArgumentNullException(nameof(idNode));
            _assExpr = assignmentExpressionNode ?? throw new ArgumentNullException(nameof(assignmentExpressionNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        public TokenNode ConstantKeywordToken => _constant;

        public TypeSyntax TypeNameNode => _type;

        public TokenNode IdentifierNameNode => _id;

        public EqualsValueClauseSyntax EqualsValueClause => _assExpr;

        public LineDelimiterSyntax LineDelimiterNode => _eol;

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new GlobalConstantDeclarationSyntax(nodes[0] as TokenNode, nodes[1] as TypeSyntax, nodes[2] as TokenNode, nodes[3] as EqualsValueClauseSyntax, nodes[4] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.ConstantKeyword));
                AddParser(TypeSyntax.Parser.Get);
                AddParser(TokenParser.Get(SyntaxTokenType.AlphanumericIdentifier));
                AddParser(EqualsValueClauseSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}