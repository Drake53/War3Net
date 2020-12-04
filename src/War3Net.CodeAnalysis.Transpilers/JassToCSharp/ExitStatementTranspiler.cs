// ------------------------------------------------------------------------------
// <copyright file="ExitStatementTranspiler.cs" company="Drake53">
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
        public static StatementSyntax Transpile(this Jass.Syntax.ExitStatementSyntax exitStatementNode)
        {
            _ = exitStatementNode ?? throw new ArgumentNullException(nameof(exitStatementNode));

            return SyntaxFactory.IfStatement(
                exitStatementNode.ConditionExpressionNode.Transpile(),
                SyntaxFactory.BreakStatement());
        }
    }
}