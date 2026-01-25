// ------------------------------------------------------------------------------
// <copyright file="TypeDeclarationRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassTypeDeclarationSyntax typeDeclaration)
        {
            Render(typeDeclaration.TypeToken);
            WriteSpace();
            Render(typeDeclaration.IdentifierName);
            WriteSpace();
            Render(typeDeclaration.ExtendsToken);
            WriteSpace();
            Render(typeDeclaration.BaseType);
        }
    }
}