// ------------------------------------------------------------------------------
// <copyright file="JassGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassGlobalDeclarationSyntax : JassSyntaxNode
    {
        protected internal override abstract JassGlobalDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal override abstract JassGlobalDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken);
    }
}