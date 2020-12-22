// ------------------------------------------------------------------------------
// <copyright file="BooleanTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("boolean")]
        public LuaExpressionSyntax? Transpile(BooleanSyntax? boolean)
        {
            if (boolean is null)
            {
                return null;
            }

            return TranspileExpression(boolean.BooleanNode);
        }
    }
}