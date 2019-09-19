// ------------------------------------------------------------------------------
// <copyright file="TokenRenderer.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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