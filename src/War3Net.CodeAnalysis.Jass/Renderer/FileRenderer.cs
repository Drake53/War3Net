// ------------------------------------------------------------------------------
// <copyright file="FileRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        public void Render(FileSyntax file)
        {
            if (file.StartFileEmpty is null)
            {
                Render(file.StartFileLineDelimiter);
            }

            Render(file.DeclarationList);
            Render(file.FunctionList);
        }
    }
}