// ------------------------------------------------------------------------------
// <copyright file="TokenRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(TokenNode token)
        {
            if (token.TokenType == SyntaxTokenType.AlphanumericIdentifier)
            {
                Write(_options.InvokeOptimizer(token.ValueText));
            }
            else
            {
                Write(token.ValueText);
            }
        }
    }
}