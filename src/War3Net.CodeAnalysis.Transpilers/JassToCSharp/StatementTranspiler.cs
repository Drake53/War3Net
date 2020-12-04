// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Jass.Syntax.StatementSyntax statementNode)
        {
            _ = statementNode ?? throw new ArgumentNullException(nameof(statementNode));

            return statementNode.SetStatementNode?.Transpile()
                ?? statementNode.CallStatementNode?.Transpile()
                ?? statementNode.IfStatementNode?.Transpile()
                ?? statementNode.LoopStatementNode?.Transpile()
                ?? statementNode.ExitStatementNode?.Transpile()
                ?? statementNode.ReturnStatementNode?.Transpile()
                ?? statementNode.DebugStatementNode.Transpile();
        }
    }
}