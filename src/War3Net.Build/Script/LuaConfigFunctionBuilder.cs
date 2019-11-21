// ------------------------------------------------------------------------------
// <copyright file="LuaConfigFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using static War3Net.Build.Providers.ConfigFunctionStatementsProvider<
    War3Net.Build.Script.LuaConfigFunctionBuilder,
    CSharpLua.LuaAst.LuaVariableListDeclarationSyntax,
    CSharpLua.LuaAst.LuaStatementSyntax,
    CSharpLua.LuaAst.LuaExpressionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class LuaConfigFunctionBuilder : LuaFunctionBuilder
    {
        public LuaConfigFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override LuaVariableListDeclarationSyntax Build()
        {
            return Build(GetConfigFunctionName, GetStatements(this).ToArray());
        }
    }
}