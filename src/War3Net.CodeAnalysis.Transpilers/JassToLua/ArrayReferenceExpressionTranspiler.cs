// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassArrayReferenceExpressionSyntax arrayReferenceExpression, out JassTypeSyntax type)
        {
            type = GetVariableType(arrayReferenceExpression.IdentifierName);

            return new LuaTableIndexAccessExpressionSyntax(
                Transpile(arrayReferenceExpression.IdentifierName),
                Transpile(arrayReferenceExpression.ElementAccessClause.Expression, out _));
        }
    }
}