// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
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
        public string Transpile(BinaryOperatorType binaryOperator, JassTypeSyntax left, JassTypeSyntax right, out JassTypeSyntax type)
        {
            switch (binaryOperator)
            {
                case BinaryOperatorType.GreaterThan:
                case BinaryOperatorType.LessThan:
                case BinaryOperatorType.Equals:
                case BinaryOperatorType.NotEquals:
                case BinaryOperatorType.GreaterOrEqual:
                case BinaryOperatorType.LessOrEqual:
                case BinaryOperatorType.And:
                case BinaryOperatorType.Or:
                    type = JassTypeSyntax.Boolean;
                    break;

                default:
                    type = left.Equals(JassTypeSyntax.String) || right.Equals(JassTypeSyntax.String)
                        ? JassTypeSyntax.String
                        : left.Equals(JassTypeSyntax.Real) || right.Equals(JassTypeSyntax.Real)
                            ? JassTypeSyntax.Real
                            : left;
                    break;
            }

            return binaryOperator switch
            {
                BinaryOperatorType.Add => type.Equals(JassTypeSyntax.String) ? LuaSyntaxNode.Tokens.Concatenation : LuaSyntaxNode.Tokens.Plus,
                BinaryOperatorType.Subtract => LuaSyntaxNode.Tokens.Sub,
                BinaryOperatorType.Multiplication => LuaSyntaxNode.Tokens.Multiply,
                BinaryOperatorType.Division => type.Equals(JassTypeSyntax.Integer) ? LuaSyntaxNode.Tokens.IntegerDiv : LuaSyntaxNode.Tokens.Div,
                BinaryOperatorType.GreaterThan => ">",
                BinaryOperatorType.LessThan => "<",
                BinaryOperatorType.Equals => LuaSyntaxNode.Tokens.EqualsEquals,
                BinaryOperatorType.NotEquals => LuaSyntaxNode.Tokens.NotEquals,
                BinaryOperatorType.GreaterOrEqual => ">=",
                BinaryOperatorType.LessOrEqual => "<=",
                BinaryOperatorType.And => LuaSyntaxNode.Keyword.And,
                BinaryOperatorType.Or => LuaSyntaxNode.Keyword.Or,
            };
        }
    }
}