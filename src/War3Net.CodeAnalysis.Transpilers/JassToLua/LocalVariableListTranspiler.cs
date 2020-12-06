// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this LocalVariableListSyntax localVariableListNode, ref StringBuilder sb)
        {
            _ = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));

            foreach (var localVariableNode in localVariableListNode)
            {
                localVariableNode.VariableDeclarationNode.TranspileLocal(ref sb);
                localVariableNode.LineDelimiterNode.Transpile(ref sb);
            }
        }

        public static IEnumerable<LuaVariableListDeclarationSyntax> TranspileToLua(this LocalVariableListSyntax localVariableListNode)
        {
            _ = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));

            return localVariableListNode.Select(localVariableNode => localVariableNode.VariableDeclarationNode.TranspileToLua(true));
        }
    }
}