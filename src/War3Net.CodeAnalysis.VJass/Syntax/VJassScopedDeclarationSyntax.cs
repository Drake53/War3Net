// ------------------------------------------------------------------------------
// <copyright file="VJassScopedDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassScopedDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassScopedDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassScopedDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}