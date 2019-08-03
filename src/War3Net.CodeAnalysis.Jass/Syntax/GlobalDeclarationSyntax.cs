// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class GlobalDeclarationSyntax : SyntaxNode
    {
        private readonly GlobalConstantDeclarationSyntax _const;
        private readonly GlobalVariableDeclarationSyntax _var;

        public GlobalDeclarationSyntax(GlobalConstantDeclarationSyntax constantGlobalNode)
            : base(constantGlobalNode)
        {
            _const = constantGlobalNode ?? throw new ArgumentNullException(nameof(constantGlobalNode));
        }

        public GlobalDeclarationSyntax(GlobalVariableDeclarationSyntax variableGlobalNode)
            : base(variableGlobalNode)
        {
            _var = variableGlobalNode ?? throw new ArgumentNullException(nameof(variableGlobalNode));
        }

        public GlobalConstantDeclarationSyntax ConstantDeclarationNode => _const;

        public GlobalVariableDeclarationSyntax VariableDeclarationNode => _var;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is GlobalConstantDeclarationSyntax constantGlobalNode)
                {
                    return new GlobalDeclarationSyntax(constantGlobalNode);
                }
                else
                {
                    return new GlobalDeclarationSyntax(node as GlobalVariableDeclarationSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(GlobalConstantDeclarationSyntax.Parser.Get);
                AddParser(GlobalVariableDeclarationSyntax.Parser.Get);

                return this;
            }
        }
    }
}