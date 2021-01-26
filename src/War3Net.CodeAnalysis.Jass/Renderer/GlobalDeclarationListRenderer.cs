// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassGlobalDeclarationListSyntax globalDeclarationList)
        {
            WriteLine(JassKeyword.Globals);

            foreach (var globalDeclaration in globalDeclarationList.Globals)
            {
                Render(globalDeclaration);
                WriteLine();
            }

            Write(JassKeyword.EndGlobals);
        }
    }
}