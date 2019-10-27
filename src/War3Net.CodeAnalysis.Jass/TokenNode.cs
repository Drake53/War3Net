// ------------------------------------------------------------------------------
// <copyright file="TokenNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    public sealed class TokenNode : SyntaxNode
    {
        private readonly SyntaxToken _token;

        public TokenNode(SyntaxToken token, int position)
            : base(position, false)
        {
            _token = token;
        }

        public SyntaxTokenType TokenType => _token.TokenType;

        public string ValueText => _token.TokenValue;

        public override string ToString()
        {
            return ValueText;
        }
    }
}