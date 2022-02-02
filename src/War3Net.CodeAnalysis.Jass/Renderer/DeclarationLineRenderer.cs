// ------------------------------------------------------------------------------
// <copyright file="DeclarationLineRenderer.cs" company="Drake53">
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
        public void Render(IDeclarationLineSyntax declarationLine)
        {
            switch (declarationLine)
            {
                case JassEmptySyntax empty: Render(empty); break;
                case JassCommentSyntax comment: Render(comment); break;
                case JassFunctionCustomScriptAction functionDeclaration: Render(functionDeclaration); break;
                case JassGlobalsCustomScriptAction globalsDeclaration: Render(globalsDeclaration); break;
                case JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration: Render(nativeFunctionDeclaration); break;
                case JassTypeDeclarationSyntax typeDeclaration: Render(typeDeclaration); break;

                default: throw new NotSupportedException();
            }
        }
    }
}