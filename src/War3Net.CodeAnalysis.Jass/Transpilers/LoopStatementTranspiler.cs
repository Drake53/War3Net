// ------------------------------------------------------------------------------
// <copyright file="LoopStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static StatementSyntax Transpile(this Syntax.LoopStatementSyntax loopStatementNode)
        {
            _ = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));

            return SyntaxFactory.WhileStatement(
                SyntaxFactory.LiteralExpression(Microsoft.CodeAnalysis.CSharp.SyntaxKind.TrueLiteralExpression),
                SyntaxFactory.Block(loopStatementNode.StatementListNode.Transpile()));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.LoopStatementSyntax loopStatementNode, ref StringBuilder sb)
        {
            _ = loopStatementNode ?? throw new ArgumentNullException(nameof(loopStatementNode));

            sb.AppendLine("while (true)");
            sb.AppendLine("do");
            loopStatementNode.StatementListNode.Transpile(ref sb);
            sb.Append("end");
        }
    }
}