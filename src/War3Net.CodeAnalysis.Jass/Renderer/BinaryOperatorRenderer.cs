// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(BinaryOperatorSyntax binaryOperator)
        {
            var tokenType = binaryOperator.BinaryOperatorToken.TokenType;
            var spaces = tokenType == SyntaxTokenType.AndOperator || tokenType == SyntaxTokenType.OrOperator || _options.OptionalWhitespace;
            if (spaces)
            {
                WriteSpace();
            }

            Render(binaryOperator.BinaryOperatorToken);
            if (spaces)
            {
                WriteSpace();
            }
        }
    }
}