// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class BinaryOperatorSyntax : SyntaxNode
    {
        private readonly TokenNode _token;

        public BinaryOperatorSyntax(TokenNode tokenNode)
            : base(tokenNode)
        {
            _token = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));
        }

        public TokenNode BinaryOperatorToken => _token;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new BinaryOperatorSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.PlusOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.MinusOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.MultiplicationOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.DivisionOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.GreaterThanOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.LessThanOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.EqualityOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.UnequalityOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.GreaterOrEqualOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.LessOrEqualOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.AndOperator));
                AddParser(TokenParser.Get(SyntaxTokenType.OrOperator));

                return this;
            }
        }
    }
}