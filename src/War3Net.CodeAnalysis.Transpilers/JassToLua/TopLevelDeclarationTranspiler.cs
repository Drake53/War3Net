// ------------------------------------------------------------------------------
// <copyright file="TopLevelDeclarationTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(JassTopLevelDeclarationSyntax declaration)
        {
            if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
            {
                RegisterNativeFunctionReturnType(nativeFunctionDeclaration);
            }

            return declaration switch
            {
                JassGlobalsDeclarationSyntax globalsDeclaration => Transpile(globalsDeclaration),
                JassFunctionDeclarationSyntax functionDeclaration => IgnoreEmptyDeclarations && KeepFunctionsSeparated ? new[] { Transpile(functionDeclaration), LuaBlankLinesStatement.One } : new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<LuaStatementSyntax>(),
            };
        }
    }
}