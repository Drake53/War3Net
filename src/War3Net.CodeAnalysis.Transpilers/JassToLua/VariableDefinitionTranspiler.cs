// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void TranspileGlobal(this VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            variableDefinitionNode.Transpile(ref sb);

            if (variableDefinitionNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterGlobalStringVariable(variableDefinitionNode.IdentifierNameNode.ValueText);
            }
        }

        [Obsolete]
        public static void TranspileLocal(this VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            sb.Append("local ");
            variableDefinitionNode.Transpile(ref sb);

            if (variableDefinitionNode.TypeNameNode.TypeNameToken.TokenType == SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterLocalStringVariable(variableDefinitionNode.IdentifierNameNode.ValueText);
            }
        }

        [Obsolete]
        private static void Transpile(this VariableDefinitionSyntax variableDefinitionNode, ref StringBuilder sb)
        {
            variableDefinitionNode.IdentifierNameNode.TranspileIdentifier(ref sb);
            if (variableDefinitionNode.EmptyEqualsValueClause is null)
            {
                variableDefinitionNode.EqualsValueClause.Transpile(ref sb);
            }
            else
            {
                sb.Append(" = nil");
            }
        }

        public static LuaVariableDeclaratorSyntax TranspileToLua(this VariableDefinitionSyntax variableDefinitionNode)
        {
            _ = variableDefinitionNode ?? throw new ArgumentNullException(nameof(variableDefinitionNode));

            return variableDefinitionNode.EqualsValueClause is null
                ? new LuaVariableDeclaratorSyntax(variableDefinitionNode.IdentifierNameNode.TranspileIdentifierToLua())
                : new LuaVariableDeclaratorSyntax(variableDefinitionNode.IdentifierNameNode.TranspileIdentifierToLua(), variableDefinitionNode.EqualsValueClause.TranspileToLua());
        }
    }
}