// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassMemberDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassMemberDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassMemberDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}