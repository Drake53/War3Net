// ------------------------------------------------------------------------------
// <copyright file="FileTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaCompilationUnitSyntax Transpile(FileSyntax file)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));

            var compilationUnit = new LuaCompilationUnitSyntax(hasGeneratedMark: false);
            if (file.StartFileLineDelimiter is not null)
            {
                compilationUnit.Statements.AddRange(Transpile(file.StartFileLineDelimiter));
            }

            compilationUnit.Statements.AddRange(Transpile(file.DeclarationList));
            compilationUnit.Statements.AddRange(file.FunctionList.SelectMany(function => Transpile(function)));

            return compilationUnit;
        }
    }
}