namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaLocalDeclarationStatementSyntax Transpile(JassVariableOrArrayDeclaratorSyntax declarator, bool isLocalDeclaration)
        {
            RegisterVariableType(declarator, isLocalDeclaration);

            var luaDeclarator = declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(arrayDeclarator),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(variableDeclarator),
            };

            var declaration = new LuaVariableListDeclarationSyntax();

            luaDeclarator.IsLocalDeclaration = isLocalDeclaration;
            declaration.Variables.Add(luaDeclarator);

            return new LuaLocalDeclarationStatementSyntax(declaration);
        }

        public LuaVariableDeclaratorSyntax Transpile(JassVariableDeclaratorSyntax variableDeclarator)
        {
            var expression = variableDeclarator.Value is null
                ? LuaIdentifierLiteralExpressionSyntax.Nil
                : Transpile(variableDeclarator.Value);

            return new LuaVariableDeclaratorSyntax(Transpile(variableDeclarator.IdentifierName), expression);
        }
    }
}