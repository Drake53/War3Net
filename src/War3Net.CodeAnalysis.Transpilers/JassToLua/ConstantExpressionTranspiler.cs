// ------------------------------------------------------------------------------
// <copyright file="ConstantExpressionTranspiler.cs" company="Drake53">
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
        [return: NotNullIfNotNull("constantExpression")]
        public LuaExpressionSyntax? Transpile(ConstantExpressionSyntax? constantExpression, out SyntaxTokenType expressionType)
        {
            if (constantExpression is null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return null;
            }

            if (constantExpression.IntegerExpressionNode is not null)
            {
                expressionType = SyntaxTokenType.IntegerKeyword;
                return Transpile(constantExpression.IntegerExpressionNode);
            }
            else if (constantExpression.RealExpressionNode is not null)
            {
                expressionType = SyntaxTokenType.RealKeyword;
                return TranspileExpression(constantExpression.RealExpressionNode);
            }
            else if (constantExpression.BooleanExpressionNode is not null)
            {
                expressionType = SyntaxTokenType.BooleanKeyword;
                return Transpile(constantExpression.BooleanExpressionNode);
            }
            else if (constantExpression.StringExpressionNode is not null)
            {
                expressionType = SyntaxTokenType.StringKeyword;
                return Transpile(constantExpression.StringExpressionNode);
            }
            else if (constantExpression.NullExpressionNode is not null)
            {
                expressionType = SyntaxTokenType.NullKeyword;
                return TranspileExpression(constantExpression.NullExpressionNode);
            }
            else
            {
                throw new ArgumentNullException(nameof(constantExpression));
            }
        }
    }
}