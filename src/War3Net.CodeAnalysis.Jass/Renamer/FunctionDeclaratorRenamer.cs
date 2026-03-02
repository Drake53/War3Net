namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameFunctionDeclarator(JassFunctionDeclaratorSyntax functionDeclarator, [NotNullWhen(true)] out JassFunctionDeclaratorSyntax? renamedFunctionDeclarator)
        {
            if (TryRenameFunctionIdentifierName(functionDeclarator.IdentifierName, out var renamedIdentifierName))
            {
                renamedFunctionDeclarator = new JassFunctionDeclaratorSyntax(
                    functionDeclarator.ConstantToken,
                    functionDeclarator.FunctionToken,
                    renamedIdentifierName,
                    functionDeclarator.ParameterList,
                    functionDeclarator.ReturnClause);

                return true;
            }

            renamedFunctionDeclarator = null;
            return false;
        }
    }
}