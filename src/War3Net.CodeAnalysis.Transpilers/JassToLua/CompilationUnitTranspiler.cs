// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaCompilationUnitSyntax Transpile(JassCompilationUnitSyntax compilationUnit)
        {
            var luaCompilationUnit = new LuaCompilationUnitSyntax(hasGeneratedMark: false);

            luaCompilationUnit.Statements.AddRange(compilationUnit.Declarations.SelectMany(Transpile));

            return luaCompilationUnit;
        }
    }
}