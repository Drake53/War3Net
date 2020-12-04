// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListTranspiler.cs" company="Drake53">
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
        public static void Transpile(this LocalVariableListSyntax localVariableListNode, ref StringBuilder sb)
        {
            _ = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));

            foreach (var localVariableNode in localVariableListNode)
            {
                localVariableNode.VariableDeclarationNode.TranspileLocal(ref sb);
                localVariableNode.LineDelimiterNode.Transpile(ref sb);
            }
        }
    }
}