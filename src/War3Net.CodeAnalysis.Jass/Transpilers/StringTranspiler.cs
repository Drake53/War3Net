// ------------------------------------------------------------------------------
// <copyright file="StringTranspiler.cs" company="Drake53">
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
        public static ExpressionSyntax Transpile(this Syntax.StringSyntax stringNode)
        {
            _ = stringNode ?? throw new ArgumentNullException(nameof(stringNode));

            return stringNode.StringNode?.TranspileExpression()
                ?? SyntaxFactory.ParseExpression("string.Empty");
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.StringSyntax stringNode, ref StringBuilder sb)
        {
            _ = stringNode ?? throw new ArgumentNullException(nameof(stringNode));

            if (stringNode.StringNode is null)
            {
                sb.Append("\"\"");
            }
            else
            {
                stringNode.StringNode.TranspileExpression(ref sb);
            }
        }
    }
}