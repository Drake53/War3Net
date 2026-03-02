// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationRenderer.cs" company="Drake53">
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
        public void Render(JassGlobalDeclarationSyntax globalDeclaration)
        {
            switch (globalDeclaration)
            {
                case JassGlobalConstantDeclarationSyntax globalConstantDeclaration: Render(globalConstantDeclaration); break;
                case JassGlobalVariableDeclarationSyntax globalVariableDeclaration: Render(globalVariableDeclaration); break;

                default: throw new NotSupportedException();
            }
        }
    }
}