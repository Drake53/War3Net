namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaExpressionSyntax Transpile(JassUnaryExpressionSyntax unaryExpression, out JassTypeSyntax type)
        {
            return new LuaPrefixUnaryExpressionSyntax(
                Transpile(unaryExpression.Expression, out type),
                TranspileUnaryExpressionKind(unaryExpression.SyntaxKind));
        }

        public string TranspileUnaryExpressionKind(JassSyntaxKind expressionKind)
        {
            return expressionKind switch
            {
                JassSyntaxKind.UnaryPlusExpression => LuaSyntaxNode.Tokens.Plus,
                JassSyntaxKind.UnaryMinusExpression => LuaSyntaxNode.Tokens.Sub,
                JassSyntaxKind.LogicalNotExpression => LuaSyntaxNode.Keyword.Not,
            };
        }
    }
}