// ------------------------------------------------------------------------------
// <copyright file="CallStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static StatementSyntax Transpile(this Syntax.CallStatementSyntax callStatementNode)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            var invocation = SyntaxFactory.InvocationExpression(
                    callStatementNode.IdentifierNameNode.TranspileExpression());

            if (callStatementNode.EmptyArgumentListNode is null)
            {
                invocation = invocation.AddArgumentListArguments(
                    callStatementNode.ArgumentListNode.Transpile().ToArray());
            }

            return SyntaxFactory.ExpressionStatement(invocation);
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.CallStatementSyntax callStatementNode, ref StringBuilder sb)
        {
            _ = callStatementNode ?? throw new ArgumentNullException(nameof(callStatementNode));

            callStatementNode.IdentifierNameNode.TranspileExpression(ref sb);
            sb.Append('(');
            if (callStatementNode.EmptyArgumentListNode is null)
            {
                callStatementNode.ArgumentListNode.Transpile(ref sb);
            }

            sb.Append(')');
        }
    }
}