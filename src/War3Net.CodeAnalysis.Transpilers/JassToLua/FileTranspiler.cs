// ------------------------------------------------------------------------------
// <copyright file="FileTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this FileSyntax fileNode, ref StringBuilder sb, bool resetStringConcatenationHandler = true)
        {
            _ = fileNode ?? throw new ArgumentNullException(nameof(fileNode));

            foreach (var declaration in fileNode.DeclarationList)
            {
                declaration.Transpile(ref sb);
            }

            foreach (var function in fileNode.FunctionList)
            {
                function.Transpile(ref sb);
            }

            if (resetStringConcatenationHandler)
            {
                TranspileStringConcatenationHandler.Reset();
            }
        }

        public static LuaCompilationUnitSyntax TranspileToLua(this FileSyntax fileNode, bool resetStringConcatenationHandler = true)
        {
            _ = fileNode ?? throw new ArgumentNullException(nameof(fileNode));

            var compilationUnit = new LuaCompilationUnitSyntax(hasGeneratedMark: false);
            compilationUnit.Statements.AddRange(fileNode.DeclarationList.TranspileToLua());
            compilationUnit.Statements.AddRange(fileNode.FunctionList.SelectMany(function => function.TranspileToLua()));

            if (resetStringConcatenationHandler)
            {
                TranspileStringConcatenationHandler.Reset();
            }

            return compilationUnit;
        }
    }
}