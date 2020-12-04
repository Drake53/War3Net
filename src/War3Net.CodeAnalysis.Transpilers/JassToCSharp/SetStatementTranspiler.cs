// ------------------------------------------------------------------------------
// <copyright file="SetStatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Jass.Syntax.SetStatementSyntax setStatementNode)
        {
            _ = setStatementNode ?? throw new ArgumentNullException(nameof(setStatementNode));

            return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                setStatementNode.ArrayIndexerNode is null
                    ? setStatementNode.IdentifierNameNode.TranspileExpression()
                    : SyntaxFactory.ElementAccessExpression(
                        setStatementNode.IdentifierNameNode.TranspileExpression(),
                        setStatementNode.ArrayIndexerNode.Transpile()),
                setStatementNode.EqualsValueClauseNode.ValueNode.Transpile()));
        }
    }
}