namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            if (functionDeclarator.ConstantToken is not null)
            {
                Render(functionDeclarator.ConstantToken);
                WriteSpace();
            }

            Render(functionDeclarator.FunctionToken);
            WriteSpace();
            Render(functionDeclarator.IdentifierName);
            WriteSpace();
            Render(functionDeclarator.ParameterList);
            WriteSpace();
            Render(functionDeclarator.ReturnClause);
        }
    }
}