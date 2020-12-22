// ------------------------------------------------------------------------------
// <copyright file="IntegerSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class IntegerSyntax : SyntaxNode
    {
        private readonly TokenNode? _token;
        private readonly FourCCIntegerSyntax? _fourCC;

        public IntegerSyntax(TokenNode tokenNode)
            : base(tokenNode)
        {
            _token = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));
        }

        public IntegerSyntax(FourCCIntegerSyntax fourCCIntegerNode)
            : base(fourCCIntegerNode)
        {
            _fourCC = fourCCIntegerNode ?? throw new ArgumentNullException(nameof(fourCCIntegerNode));
        }

        public TokenNode? IntegerToken => _token;

        public FourCCIntegerSyntax? FourCCIntegerNode => _fourCC;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is FourCCIntegerSyntax fourCC)
                {
                    return new IntegerSyntax(fourCC);
                }
                else
                {
                    return new IntegerSyntax(node as TokenNode);
                }
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.DecimalNumber));
                AddParser(TokenParser.Get(SyntaxTokenType.OctalNumber));
                AddParser(TokenParser.Get(SyntaxTokenType.HexadecimalNumber));
                AddParser(FourCCIntegerSyntax.Parser.Get);

                return this;
            }
        }
    }
}