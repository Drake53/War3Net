﻿// ------------------------------------------------------------------------------
// <copyright file="FunctionReferenceExpressionTranspiler.cs" company="Drake53">
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
        public LuaExpressionSyntax Transpile(JassFunctionReferenceExpressionSyntax functionReferenceExpression, out JassTypeSyntax type)
        {
            type = JassPredefinedTypeSyntax.Code;

            return Transpile(functionReferenceExpression.IdentifierName);
        }
    }
}