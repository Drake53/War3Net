// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax Transpile(this Jass.Syntax.GlobalDeclarationSyntax globalDeclarationNode)
        {
            _ = globalDeclarationNode ?? throw new ArgumentNullException(nameof(globalDeclarationNode));

            return globalDeclarationNode.ConstantDeclarationNode?.Transpile()
                ?? globalDeclarationNode.VariableDeclarationNode.Transpile();
        }
    }
}