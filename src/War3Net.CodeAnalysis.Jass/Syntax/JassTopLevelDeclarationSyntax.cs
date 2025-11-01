// ------------------------------------------------------------------------------
// <copyright file="JassTopLevelDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassTopLevelDeclarationSyntax : JassSyntaxNode
    {
        protected internal override abstract JassTopLevelDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassTopLevelDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}