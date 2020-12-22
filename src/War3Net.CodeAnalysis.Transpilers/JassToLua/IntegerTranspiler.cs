// ------------------------------------------------------------------------------
// <copyright file="IntegerTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        [return: NotNullIfNotNull("integer")]
        public LuaExpressionSyntax? Transpile(IntegerSyntax? integer)
        {
            if (integer is null)
            {
                return null;
            }

            if (integer.FourCCIntegerNode is not null)
            {
                return Transpile(integer.FourCCIntegerNode);
            }
            else if (integer.IntegerToken is not null)
            {
                return TranspileExpression(integer.IntegerToken);
            }
            else
            {
                throw new ArgumentNullException(nameof(integer));
            }
        }
    }
}