// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Syntax.ArrayReferenceSyntax arrayReferenceNode)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }

        public static ExpressionSyntax Transpile(this Syntax.ArrayReferenceSyntax arrayReferenceNode, out bool @const)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            @const = false;

            return SyntaxFactory.ElementAccessExpression(
                arrayReferenceNode.IdentifierNameNode.TranspileExpression())
                .AddArgumentListArguments(
                SyntaxFactory.Argument(
                    arrayReferenceNode.IndexExpressionNode.Transpile()));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.ArrayReferenceSyntax arrayReferenceNode, ref StringBuilder sb, out bool isString)
        {
            _ = arrayReferenceNode ?? throw new ArgumentNullException(nameof(arrayReferenceNode));

            isString = TranspileStringConcatenationHandler.IsStringVariable(arrayReferenceNode.IdentifierNameNode.ValueText);

            arrayReferenceNode.IdentifierNameNode.TranspileExpression(ref sb);
            sb.Append('[');
            arrayReferenceNode.IndexExpressionNode.Transpile(ref sb);
            sb.Append(']');
        }
    }
}