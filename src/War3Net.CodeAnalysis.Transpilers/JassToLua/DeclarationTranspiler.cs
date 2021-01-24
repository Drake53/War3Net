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
                JassEmptyDeclarationSyntax emptyDeclaration => new[] { Transpile(emptyDeclaration) },
                JassCommentDeclarationSyntax commentDeclaration => new[] { Transpile(commentDeclaration) },
                JassGlobalDeclarationListSyntax globalDeclarationList => Transpile(globalDeclarationList),
                JassGlobalDeclarationSyntax globalDeclaration => new[] { Transpile(globalDeclaration) },
                JassFunctionDeclarationSyntax functionDeclaration => new[] { Transpile(functionDeclaration) },
                _ => Array.Empty<LuaStatementSyntax>(),
            };
        }
    }
}