// ------------------------------------------------------------------------------
// <copyright file="IfStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static StatementSyntax Transpile(this Jass.Syntax.IfStatementSyntax ifStatementNode)
        {
            _ = ifStatementNode ?? throw new ArgumentNullException(nameof(ifStatementNode));

            var ifStatement = SyntaxFactory.IfStatement(
                ifStatementNode.ConditionExpressionNode.Transpile(),
                SyntaxFactory.Block(ifStatementNode.StatementListNode.Transpile()));

            return ifStatementNode.EmptyElseClauseNode is null
                ? ifStatement.WithElse(ifStatementNode.ElseClauseNode.Transpile())
                : ifStatement;
        }
    }
}