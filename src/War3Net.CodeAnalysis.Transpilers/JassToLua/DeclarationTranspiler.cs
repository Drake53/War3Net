// ------------------------------------------------------------------------------
// <copyright file="DeclarationTranspiler.cs" company="Drake53">
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
        public IEnumerable<LuaStatementSyntax> Transpile(IDeclarationSyntax declaration)
        {
            if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
            {
                RegisterFunctionReturnType(nativeFunctionDeclaration.FunctionDeclarator);
            }

            return declaration switch
            {
                JassEmptyDeclarationSyntax emptyDeclaration => IgnoreEmptyDeclarations ? Array.Empty<LuaStatementSyntax>() : new[] { Transpile(emptyDeclaration) },
                JassCommentDeclarationSyntax commentDeclaration => IgnoreComments ? Array.Empty<LuaStatementSyntax>() : new[] { Transpile(commentDeclaration) },
                JassGlobalDeclarationListSyntax globalDeclarationList => Transpile(globalDeclarationList),
                JassGlobalDeclarationSyntax globalDeclaration => new[] { Transpile(globalDeclaration) },
                JassFunctionDeclarationSyntax functionDeclaration => IgnoreEmptyDeclarations && KeepFunctionsSeparated ? new[] { Transpile(functionDeclaration), LuaBlankLinesStatement.One } : new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<LuaStatementSyntax>(),
            };
        }
    }
}