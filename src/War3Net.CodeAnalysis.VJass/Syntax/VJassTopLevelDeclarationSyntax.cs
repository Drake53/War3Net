// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassTopLevelDeclarationSyntax : VJassSyntaxNode
    {
        protected internal override abstract VJassTopLevelDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal override abstract VJassTopLevelDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken);
    }
}