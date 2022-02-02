// ------------------------------------------------------------------------------
// <copyright file="GlobalLineRenderer.cs" company="Drake53">
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
        public void Render(IGlobalLineSyntax globalLine)
        {
            switch (globalLine)
            {
                case JassEmptySyntax empty: Render(empty); break;
                case JassCommentSyntax comment: Render(comment); break;
                case JassGlobalDeclarationSyntax globalDeclaration: Render(globalDeclaration); break;
                case JassEndGlobalsCustomScriptAction endGlobals: Render(endGlobals); break;

                default: throw new NotSupportedException();
            }
        }
    }
}