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
        public IEnumerable<LuaStatementSyntax> Transpile(DeclarationSyntax declaration)
        {
            _ = declaration ?? throw new ArgumentNullException(nameof(declaration));

            if (declaration.GlobalsBlock is not null)
            {
                return Transpile(declaration.GlobalsBlock);
            }
            else
            {
                if (declaration.NativeFunctionDeclaration is not null)
                {
                    RegisterFunctionReturnType(declaration.NativeFunctionDeclaration.FunctionDeclarationNode);
                }

                // TypeDefinition and NativeFunctionDeclaration cannot be transpiled to lua.
                return Array.Empty<LuaStatementSyntax>();
            }
        }
    }
}