// ------------------------------------------------------------------------------
// <copyright file="GlobalsDeclarationRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalsDeclarationSyntax globalsDeclaration)
        {
            Render(globalsDeclaration.GlobalsToken);
            Indent();

            foreach (var globalDeclaration in globalsDeclaration.GlobalDeclarations)
            {
                Render(globalDeclaration);
            }

            Outdent();
            Render(globalsDeclaration.EndGlobalsToken);
        }
    }
}