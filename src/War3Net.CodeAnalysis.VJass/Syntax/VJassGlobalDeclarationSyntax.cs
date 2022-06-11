// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassGlobalDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}