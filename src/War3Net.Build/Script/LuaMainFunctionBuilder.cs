// ------------------------------------------------------------------------------
// <copyright file="LuaMainFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using War3Net.Build.Providers;

using static War3Net.Build.Providers.MainFunctionStatementsProvider<
    War3Net.Build.Script.LuaMainFunctionBuilder,
    CSharpLua.LuaAst.LuaVariableListDeclarationSyntax,
    CSharpLua.LuaAst.LuaStatementSyntax,
    CSharpLua.LuaAst.LuaExpressionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class LuaMainFunctionBuilder : LuaFunctionBuilder
    {
        public LuaMainFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override LuaVariableListDeclarationSyntax Build()
        {
            return Build(GetMainFunctionName, GetStatements(this).ToArray());
        }
    }
}