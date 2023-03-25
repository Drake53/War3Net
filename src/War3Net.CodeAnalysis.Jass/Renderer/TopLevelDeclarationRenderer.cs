// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationRenderer.cs" company="Drake53">
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
        public void Render(JassTopLevelDeclarationSyntax declaration)
        {
            switch (declaration)
            {
                case JassTypeDeclarationSyntax typeDeclaration: Render(typeDeclaration); break;
                case JassGlobalsDeclarationSyntax globalDeclarationList: Render(globalDeclarationList); break;
                case JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration: Render(nativeFunctionDeclaration); break;
                case JassFunctionDeclarationSyntax functionDeclaration: Render(functionDeclaration); break;

                default: throw new NotSupportedException();
            }
        }
    }
}