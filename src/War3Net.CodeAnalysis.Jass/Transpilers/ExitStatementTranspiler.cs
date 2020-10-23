// ------------------------------------------------------------------------------
// <copyright file="ExitStatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Syntax.ExitStatementSyntax exitStatementNode)
        {
            _ = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));

            return SyntaxFactory.IfStatement(
                exitStatementNode.ConditionExpressionNode.Transpile(),
                SyntaxFactory.BreakStatement());
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ExitStatementSyntax exitStatementNode, ref StringBuilder sb)
        {
            _ = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));

            sb.Append("if ");
            exitStatementNode.ConditionExpressionNode.Transpile(ref sb);
            sb.AppendLine(" then");
            sb.AppendLine("break");
            sb.Append("end");
        }
    }
}