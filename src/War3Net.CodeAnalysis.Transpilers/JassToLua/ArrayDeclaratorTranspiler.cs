namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaVariableDeclaratorSyntax Transpile(JassArrayDeclaratorSyntax arrayDeclarator)
        {
            LuaExpressionSyntax expression = arrayDeclarator.Type.Token.SyntaxKind switch
            {
                JassSyntaxKind.IntegerKeyword => new LuaInvocationExpressionSyntax("__jarray", "0"),
                JassSyntaxKind.RealKeyword => new LuaInvocationExpressionSyntax("__jarray", "0.0"),
                JassSyntaxKind.StringKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaStringLiteralExpressionSyntax.Empty),
                JassSyntaxKind.BooleanKeyword => new LuaInvocationExpressionSyntax("__jarray", LuaIdentifierLiteralExpressionSyntax.False),
                _ => new LuaTableExpression(),
            };

            return new LuaVariableDeclaratorSyntax(Transpile(arrayDeclarator.IdentifierName), expression);
        }
    }
}