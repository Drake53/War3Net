// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax Transpile(this Syntax.GlobalDeclarationSyntax globalDeclarationNode)
        {
            _ = globalDeclarationNode ?? throw new ArgumentNullException(nameof(globalDeclarationNode));

            return globalDeclarationNode.ConstantDeclarationNode?.Transpile()
                ?? globalDeclarationNode.VariableDeclarationNode.Transpile();
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.GlobalDeclarationSyntax globalDeclarationNode, ref StringBuilder sb)
        {
            _ = globalDeclarationNode ?? throw new ArgumentNullException(nameof(globalDeclarationNode));

            globalDeclarationNode.ConstantDeclarationNode?.Transpile(ref sb);
            globalDeclarationNode.VariableDeclarationNode?.Transpile(ref sb);
        }
    }
}