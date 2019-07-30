// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class GlobalVariableDeclarationSyntax : SyntaxNode
    {
        private readonly VariableDeclarationSyntax _declr;
        private readonly LineDelimiterSyntax _eol;

        public GlobalVariableDeclarationSyntax(VariableDeclarationSyntax variableDeclarationNode, LineDelimiterSyntax eolNode)
            : base(variableDeclarationNode, eolNode)
        {
            _declr = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));
            _eol = eolNode ?? throw new ArgumentNullException(nameof(eolNode));
        }

        internal sealed class Parser : SequenceParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(List<SyntaxNode> nodes)
            {
                return new GlobalVariableDeclarationSyntax(nodes[0] as VariableDeclarationSyntax, nodes[1] as LineDelimiterSyntax);
            }

            private Parser Init()
            {
                AddParser(VariableDeclarationSyntax.Parser.Get);
                AddParser(LineDelimiterSyntax.Parser.Get);

                return this;
            }
        }
    }
}