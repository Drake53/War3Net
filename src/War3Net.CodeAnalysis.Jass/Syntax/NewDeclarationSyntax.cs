// ------------------------------------------------------------------------------
// <copyright file="NewDeclarationSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class NewDeclarationSyntax : SyntaxNode
    {
        private readonly DeclarationSyntax _declr;
        private readonly LineDelimiterSyntax _eol;

        public NewDeclarationSyntax(DeclarationSyntax declarationNode, LineDelimiterSyntax eolNode)
            : base(declarationNode, eolNode)
        {
            _declr = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new NewDeclarationSyntax(nodes[0] as DeclarationSyntax, nodes[1] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(DeclarationSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}