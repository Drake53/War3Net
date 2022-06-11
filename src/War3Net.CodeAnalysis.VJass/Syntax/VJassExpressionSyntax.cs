// ------------------------------------------------------------------------------
// <copyright file="VJassExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassExpressionSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}