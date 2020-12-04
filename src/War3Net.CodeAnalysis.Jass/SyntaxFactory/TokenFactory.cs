// ------------------------------------------------------------------------------
// <copyright file="TokenFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static TokenNode Token(SyntaxTokenType tokenType)
        {
            return new TokenNode(new SyntaxToken(tokenType), 0);
        }

        public static TokenNode Token(SyntaxTokenType tokenType, string tokenValue)
        {
            return new TokenNode(new SyntaxToken(tokenType, tokenValue), 0);
        }

        public static EmptyNode Empty()
        {
            return new EmptyNode(0);
        }
    }
}