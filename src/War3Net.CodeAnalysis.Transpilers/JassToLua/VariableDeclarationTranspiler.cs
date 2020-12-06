// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void TranspileGlobal(this VariableDeclarationSyntax variableDeclarationNode, ref StringBuilder sb)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            variableDeclarationNode.VariableDefinitionNode?.TranspileGlobal(ref sb);
            variableDeclarationNode.ArrayDefinitionNode?.TranspileGlobal(ref sb);
        }

        [Obsolete]
        public static void TranspileLocal(this VariableDeclarationSyntax variableDeclarationNode, ref StringBuilder sb)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            variableDeclarationNode.VariableDefinitionNode?.TranspileLocal(ref sb);
            variableDeclarationNode.ArrayDefinitionNode?.TranspileLocal(ref sb);
        }

        public static LuaVariableListDeclarationSyntax TranspileToLua(this VariableDeclarationSyntax variableDeclarationNode, bool isLocalDeclaration)
        {
            _ = variableDeclarationNode ?? throw new ArgumentNullException(nameof(variableDeclarationNode));

            var declarator = variableDeclarationNode.VariableDefinitionNode?.TranspileToLua() ?? variableDeclarationNode.ArrayDefinitionNode.TranspileToLua();
            declarator.IsLocalDeclaration = isLocalDeclaration;

            var declaration = new LuaVariableListDeclarationSyntax();
            declaration.Variables.Add(declarator);
            return declaration;
        }
    }
}