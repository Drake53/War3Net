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
        public void Render(IVariableDeclaratorSyntax declarator)
        {
            switch (declarator)
            {
                case JassArrayDeclaratorSyntax arrayDeclarator: Render(arrayDeclarator); break;
                case JassVariableDeclaratorSyntax variableDeclarator: Render(variableDeclarator); break;
            }
        }

        public void Render(JassVariableDeclaratorSyntax variableDeclarator)
        {
            Render(variableDeclarator.Type);
            Write(' ');
            Render(variableDeclarator.IdentifierName);

            if (variableDeclarator.Value is not null)
            {
                Write(' ');
                Render(variableDeclarator.Value);
            }
        }
    }
}