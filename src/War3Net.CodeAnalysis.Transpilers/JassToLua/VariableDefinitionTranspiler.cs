// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaVariableDeclaratorSyntax Transpile(VariableDefinitionSyntax variableDefinition)
        {
            _ = variableDefinition ?? throw new ArgumentNullException(nameof(variableDefinition));

            var identifier = TranspileIdentifier(variableDefinition.IdentifierNameNode);
            var expression = variableDefinition.EqualsValueClause is null
                ? LuaIdentifierLiteralExpressionSyntax.Nil
                : Transpile(variableDefinition.EqualsValueClause);

            return new LuaVariableDeclaratorSyntax(identifier, expression);
        }
    }
}