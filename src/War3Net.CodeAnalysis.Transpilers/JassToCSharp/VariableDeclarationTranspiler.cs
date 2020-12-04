// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static MemberDeclarationSyntax TranspileMember(this Jass.Syntax.VariableDeclarationSyntax variableDeclarationNode)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            return variableDeclarationNode.VariableDefinitionNode?.TranspileMember()
                ?? variableDeclarationNode.ArrayDefinitionNode.TranspileMember();
        }

        public static LocalDeclarationStatementSyntax TranspileLocal(this Jass.Syntax.VariableDeclarationSyntax variableDeclarationNode)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            return variableDeclarationNode.VariableDefinitionNode?.TranspileLocal()
                ?? variableDeclarationNode.ArrayDefinitionNode.TranspileLocal();
        }
    }
}