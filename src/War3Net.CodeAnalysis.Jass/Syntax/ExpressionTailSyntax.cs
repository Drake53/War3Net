// ------------------------------------------------------------------------------
// <copyright file="ExpressionTailSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete("Don't make subclasses for what is essentially an optional parser. (use new OptionalParser(BinaryExpressionTailSyntax.Parser.Get))", true)]
    public sealed class ExpressionTailSyntax : SyntaxNode
    {
        private readonly BinaryExpressionTailSyntax _expression;
        private readonly EmptyNode _empty;

        public ExpressionTailSyntax(BinaryExpressionTailSyntax expressionNode)
            : base(expressionNode)
        {
            _expression = expressionNode ?? throw new ArgumentNullException(nameof(expressionNode));
        }

        public ExpressionTailSyntax(EmptyNode emptyNode)
            : base(emptyNode)
        {
            _empty = emptyNode ?? throw new ArgumentNullException(nameof(emptyNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is EmptyNode emptyNode)
                {
                    return new ExpressionTailSyntax(node as EmptyNode);
                }
                else
                {
                    return new ExpressionTailSyntax(node as BinaryExpressionTailSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(BinaryExpressionTailSyntax.Parser.Get);
                AddParser(EmptyParser.Get);

                return this;
            }
        }
    }
}