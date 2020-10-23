// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<StatementSyntax> Transpile(this Syntax.LocalVariableListSyntax localVariableListNode)
        {
            _ = localVariableListNode ?? throw new ArgumentNullException(nameof(localVariableListNode));

            return localVariableListNode.Select(localVariable => localVariable.VariableDeclarationNode.TranspileLocal());
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.LocalVariableListSyntax localVariableListNode, ref StringBuilder sb)
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