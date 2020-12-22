// ------------------------------------------------------------------------------
// <copyright file="ElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class ElseClauseSyntax : SyntaxNode
    {
        private readonly ElseifSyntax? _elseif;
        private readonly ElseSyntax? _else;

        public ElseClauseSyntax(ElseifSyntax elseifNode)
            : base(elseifNode)
        {
            _elseif = elseifNode ?? throw new ArgumentNullException(nameof(elseifNode));
        }

        public ElseClauseSyntax(ElseSyntax elseNode)
            : base(elseNode)
        {
            _else = elseNode ?? throw new ArgumentNullException(nameof(elseNode));
        }

        public ElseifSyntax? ElseifNode => _elseif;

        public ElseSyntax? ElseNode => _else;

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                if (node is ElseifSyntax elseifNode)
                {
                    return new ElseClauseSyntax(elseifNode);
                }
                else
                {
                    return new ElseClauseSyntax(node as ElseSyntax);
                }
            }

            private Parser Init()
            {
                AddParser(ElseifSyntax.Parser.Get);
                AddParser(ElseSyntax.Parser.Get);

                return this;
            }
        }
    }
}