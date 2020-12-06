// ------------------------------------------------------------------------------
// <copyright file="DebugStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this DebugStatementSyntax debugStatementNode, ref StringBuilder sb)
        {
            _ = debugStatementNode ?? throw new ArgumentNullException(nameof(debugStatementNode));

            // TODO: implement
        }

        public static LuaStatementSyntax TranspileToLua(this DebugStatementSyntax debugStatementNode)
        {
            _ = debugStatementNode ?? throw new ArgumentNullException(nameof(debugStatementNode));

            throw new NotImplementedException();
        }
    }
}