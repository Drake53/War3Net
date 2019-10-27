// ------------------------------------------------------------------------------
// <copyright file="LocalVariableListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;

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
}