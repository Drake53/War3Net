// ------------------------------------------------------------------------------
// <copyright file="StatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Syntax.StatementSyntax statementNode)
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

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.StatementSyntax statementNode, ref StringBuilder sb)
        {
            _ = statementNode ?? throw new ArgumentNullException(nameof(statementNode));

            statementNode.SetStatementNode?.Transpile(ref sb);
            statementNode.CallStatementNode?.Transpile(ref sb);
            statementNode.IfStatementNode?.Transpile(ref sb);
            statementNode.LoopStatementNode?.Transpile(ref sb);
            statementNode.ExitStatementNode?.Transpile(ref sb);
            statementNode.ReturnStatementNode?.Transpile(ref sb);
            statementNode.DebugStatementNode?.Transpile(ref sb);
        }
    }
}