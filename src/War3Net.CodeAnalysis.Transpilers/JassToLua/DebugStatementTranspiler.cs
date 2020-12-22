// ------------------------------------------------------------------------------
// <copyright file="DebugStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("debugStatement")]
        public LuaStatementSyntax? Transpile(DebugStatementSyntax? debugStatement)
        {
            if (debugStatement is null)
            {
                return null;
            }

            throw new NotImplementedException();
        }
    }
}