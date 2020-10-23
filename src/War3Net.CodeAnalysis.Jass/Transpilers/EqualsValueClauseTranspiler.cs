// ------------------------------------------------------------------------------
// <copyright file="EqualsValueClauseTranspiler.cs" company="Drake53">
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
        public static EqualsValueClauseSyntax Transpile(this Syntax.EqualsValueClauseSyntax equalsValueClauseNode)
        {
            _ = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));

            return SyntaxFactory.EqualsValueClause(equalsValueClauseNode.ValueNode.Transpile());
        }

        public static EqualsValueClauseSyntax Transpile(this Syntax.EqualsValueClauseSyntax equalsValueClauseNode, out bool @const)
        {
            _ = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));

            return SyntaxFactory.EqualsValueClause(equalsValueClauseNode.ValueNode.Transpile(out @const));
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.EqualsValueClauseSyntax equalsValueClauseNode, ref StringBuilder sb)
        {
            _ = equalsValueClauseNode ?? throw new ArgumentNullException(nameof(equalsValueClauseNode));

            sb.Append(" = ");
            equalsValueClauseNode.ValueNode.Transpile(ref sb);
        }
    }
}