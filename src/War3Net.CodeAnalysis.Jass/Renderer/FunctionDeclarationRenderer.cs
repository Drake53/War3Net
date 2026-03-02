namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassFunctionDeclarationSyntax functionDeclaration)
        {
            Render(functionDeclaration.FunctionDeclarator);
            Indent();
            Render(functionDeclaration.Statements);
            Outdent();
            Render(functionDeclaration.EndFunctionToken);
        }
    }
}