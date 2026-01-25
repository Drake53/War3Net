// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            Render(globalConstantDeclaration.ConstantToken);
            WriteSpace();
            Render(globalConstantDeclaration.Type);
            WriteSpace();
            Render(globalConstantDeclaration.IdentifierName);
            WriteSpace();
            Render(globalConstantDeclaration.Value);
        }
    }
}