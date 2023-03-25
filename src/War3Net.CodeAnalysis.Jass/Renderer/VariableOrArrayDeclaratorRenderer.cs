// ------------------------------------------------------------------------------
// <copyright file="VariableOrArrayDeclaratorRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassVariableOrArrayDeclaratorSyntax declarator)
        {
            switch (declarator)
            {
                case JassArrayDeclaratorSyntax arrayDeclarator: Render(arrayDeclarator); break;
                case JassVariableDeclaratorSyntax variableDeclarator: Render(variableDeclarator); break;

                default: throw new NotSupportedException();
            }
        }
    }
}