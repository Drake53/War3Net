// ------------------------------------------------------------------------------
// <copyright file="DeclarationRenderer.cs" company="Drake53">
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
        public void Render(ITopLevelDeclarationSyntax declaration)
        {
            switch (declaration)
            {
                case JassEmptySyntax empty: Render(empty); break;
                case JassCommentSyntax comment: Render(comment); break;
                case JassTypeDeclarationSyntax typeDeclaration: Render(typeDeclaration); break;
                case JassGlobalDeclarationListSyntax globalDeclarationList: Render(globalDeclarationList); break;
                case JassGlobalDeclarationSyntax globalDeclaration: Render(globalDeclaration); break;
                case JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration: Render(nativeFunctionDeclaration); break;
                case JassFunctionDeclarationSyntax functionDeclaration: Render(functionDeclaration); break;

                default: throw new NotSupportedException();
            }
        }
    }
}