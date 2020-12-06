// ------------------------------------------------------------------------------
// <copyright file="GlobalVariableDeclarationTranspiler.cs" company="Drake53">
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
        public static void Transpile(this GlobalVariableDeclarationSyntax globalVariableDeclarationNode, ref StringBuilder sb)
        {
            _ = globalVariableDeclarationNode ?? throw new ArgumentNullException(nameof(globalVariableDeclarationNode));

            globalVariableDeclarationNode.DeclarationNode.TranspileGlobal(ref sb);
            globalVariableDeclarationNode.LineDelimiterNode.Transpile(ref sb);
        }

        public static LuaVariableListDeclarationSyntax TranspileToLua(this GlobalVariableDeclarationSyntax globalVariableDeclarationNode)
        {
            _ = globalVariableDeclarationNode ?? throw new ArgumentNullException(nameof(globalVariableDeclarationNode));

            return globalVariableDeclarationNode.DeclarationNode.TranspileToLua(false);
        }
    }
}