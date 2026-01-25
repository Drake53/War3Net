// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassVariableDeclaratorSyntax variableDeclarator)
        {
            Render(variableDeclarator.Type);
            WriteSpace();
            Render(variableDeclarator.IdentifierName);

            if (variableDeclarator.Value is not null)
            {
                WriteSpace();
                Render(variableDeclarator.Value);
            }
        }
    }
}