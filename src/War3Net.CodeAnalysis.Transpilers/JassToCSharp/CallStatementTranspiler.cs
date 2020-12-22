// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static StatementSyntax Transpile(this Jass.Syntax.CallStatementSyntax callStatementNode)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            var invocation = SyntaxFactory.InvocationExpression(
                    callStatementNode.IdentifierNameNode.TranspileExpression());

            if (callStatementNode.ArgumentListNode is not null)
            {
                invocation = invocation.AddArgumentListArguments(
                    callStatementNode.ArgumentListNode.Transpile().ToArray());
            }

            return SyntaxFactory.ExpressionStatement(invocation);
        }
    }
}