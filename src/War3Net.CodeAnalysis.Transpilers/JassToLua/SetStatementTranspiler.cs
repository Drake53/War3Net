// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
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
        public static void Transpile(this SetStatementSyntax setStatementNode, ref StringBuilder sb)
        {
            _ = setStatementNode ?? throw new ArgumentNullException(nameof(setStatementNode));

            setStatementNode.IdentifierNameNode.TranspileExpression(ref sb);
            if (setStatementNode.EmptyArrayIndexerNode is null)
            {
                setStatementNode.ArrayIndexerNode.Transpile(ref sb);
            }

            setStatementNode.EqualsValueClauseNode.Transpile(ref sb);
        }
    }
}