// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this GlobalDeclarationSyntax globalDeclarationNode, ref StringBuilder sb)
        {
            _ = globalDeclarationNode ?? throw new ArgumentNullException(nameof(globalDeclarationNode));

            globalDeclarationNode.ConstantDeclarationNode?.Transpile(ref sb);
            globalDeclarationNode.VariableDeclarationNode?.Transpile(ref sb);
        }

        public static LuaVariableListDeclarationSyntax TranspileToLua(this GlobalDeclarationSyntax globalDeclarationNode)
        {
            _ = globalDeclarationNode ?? throw new ArgumentNullException(nameof(globalDeclarationNode));

            return globalDeclarationNode.ConstantDeclarationNode?.TranspileToLua()
                ?? globalDeclarationNode.VariableDeclarationNode.TranspileToLua();
        }
    }
}