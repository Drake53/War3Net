// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class VariableDeclarationSyntax : SyntaxNode
    {
        private readonly VariableDefinitionSyntax _var;
        private readonly ArrayDefinitionSyntax _array;

        public VariableDeclarationSyntax(VariableDefinitionSyntax variableNode)
            : base(variableNode)
        {
            _var = variableNode ?? throw new ArgumentNullException(nameof(variableNode));
        }

        public VariableDeclarationSyntax(ArrayDefinitionSyntax arrayNode)
            : base(arrayNode)
        {
            _array = arrayNode ?? throw new ArgumentNullException(nameof(arrayNode));
        }

        public VariableDefinitionSyntax VariableDefinitionNode => _var;

        public ArrayDefinitionSyntax ArrayDefinitionNode => _array;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is VariableDefinitionSyntax variableDefinitionNode)
                {
                    return new VariableDeclarationSyntax(variableDefinitionNode);
                }
                else
                {
                    return new VariableDeclarationSyntax(node as ArrayDefinitionSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(VariableDefinitionSyntax.Parser.Get);
                AddParser(ArrayDefinitionSyntax.Parser.Get);

                return this;
            }
        }
    }
}