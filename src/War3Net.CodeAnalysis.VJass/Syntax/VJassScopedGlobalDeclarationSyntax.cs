// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassScopedGlobalDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassScopedGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassScopedGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}