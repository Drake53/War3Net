// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax TranspileMember(this Syntax.VariableDeclarationSyntax variableDeclarationNode)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            return variableDeclarationNode.VariableDefinitionNode?.TranspileMember()
                ?? variableDeclarationNode.ArrayDefinitionNode.TranspileMember();
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Syntax.VariableDeclarationSyntax variableDeclarationNode)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            return variableDeclarationNode.VariableDefinitionNode?.TranspileLocal()
                ?? variableDeclarationNode.ArrayDefinitionNode.TranspileLocal();
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void TranspileGlobal(this Syntax.VariableDeclarationSyntax variableDeclarationNode, ref StringBuilder sb)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            variableDeclarationNode.VariableDefinitionNode?.TranspileGlobal(ref sb);
            variableDeclarationNode.ArrayDefinitionNode?.TranspileGlobal(ref sb);
        }

        public static void TranspileLocal(this Syntax.VariableDeclarationSyntax variableDeclarationNode, ref StringBuilder sb)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            variableDeclarationNode.VariableDefinitionNode?.TranspileLocal(ref sb);
            variableDeclarationNode.ArrayDefinitionNode?.TranspileLocal(ref sb);
        }
    }
}