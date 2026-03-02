namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression, out JassTypeSyntax type)
        {
            var left = Transpile(binaryExpression.Left, out var leftType);
            var right = Transpile(binaryExpression.Right, out var rightType);

            return new LuaBinaryExpressionSyntax(left, TranspileBinaryExpressionKind(binaryExpression.SyntaxKind, leftType, rightType, out type), right);
        }

        public string TranspileBinaryExpressionKind(JassSyntaxKind expressionKind, JassTypeSyntax left, JassTypeSyntax right, out JassTypeSyntax type)
        {
            switch (expressionKind)
            {
                case JassSyntaxKind.GreaterThanExpression:
                case JassSyntaxKind.LessThanExpression:
                case JassSyntaxKind.EqualsExpression:
                case JassSyntaxKind.NotEqualsExpression:
                case JassSyntaxKind.GreaterThanOrEqualExpression:
                case JassSyntaxKind.LessThanOrEqualExpression:
                case JassSyntaxKind.LogicalAndExpression:
                case JassSyntaxKind.LogicalOrExpression:
                    type = JassPredefinedTypeSyntax.Boolean;
                    break;

                default:
                    type = left.IsEquivalentTo(JassPredefinedTypeSyntax.String) || right.IsEquivalentTo(JassPredefinedTypeSyntax.String)
                        ? JassPredefinedTypeSyntax.String
                        : left.IsEquivalentTo(JassPredefinedTypeSyntax.Real) || right.IsEquivalentTo(JassPredefinedTypeSyntax.Real)
                            ? JassPredefinedTypeSyntax.Real
                            : left;
                    break;
            }

            return expressionKind switch
            {
                JassSyntaxKind.AddExpression => type.IsEquivalentTo(JassPredefinedTypeSyntax.String) ? LuaSyntaxNode.Tokens.Concatenation : LuaSyntaxNode.Tokens.Plus,
                JassSyntaxKind.SubtractExpression => LuaSyntaxNode.Tokens.Sub,
                JassSyntaxKind.MultiplyExpression => LuaSyntaxNode.Tokens.Multiply,
                JassSyntaxKind.DivideExpression => type.IsEquivalentTo(JassPredefinedTypeSyntax.Integer) ? LuaSyntaxNode.Tokens.IntegerDiv : LuaSyntaxNode.Tokens.Div,
                JassSyntaxKind.GreaterThanExpression => ">",
                JassSyntaxKind.LessThanExpression => "<",
                JassSyntaxKind.EqualsExpression => LuaSyntaxNode.Tokens.EqualsEquals,
                JassSyntaxKind.NotEqualsExpression => LuaSyntaxNode.Tokens.NotEquals,
                JassSyntaxKind.GreaterThanOrEqualExpression => ">=",
                JassSyntaxKind.LessThanOrEqualExpression => "<=",
                JassSyntaxKind.LogicalAndExpression => LuaSyntaxNode.Keyword.And,
                JassSyntaxKind.LogicalOrExpression => LuaSyntaxNode.Keyword.Or,
            };
        }
    }
}