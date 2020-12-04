// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this GlobalConstantDeclarationSyntax globalConstantDeclarationNode, ref StringBuilder sb)
        {
            _ = globalConstantDeclarationNode ?? throw new ArgumentNullException(nameof(globalConstantDeclarationNode));

            globalConstantDeclarationNode.IdentifierNameNode.TranspileIdentifier(ref sb);
            globalConstantDeclarationNode.EqualsValueClause.Transpile(ref sb);
            globalConstantDeclarationNode.LineDelimiterNode.Transpile(ref sb);
        }
    }
}