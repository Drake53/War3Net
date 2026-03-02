using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameFunctionDeclaration(JassFunctionDeclarationSyntax functionDeclaration, [NotNullWhen(true)] out JassTopLevelDeclarationSyntax? renamedFunctionDeclaration)
        {
            foreach (var parameter in functionDeclaration.FunctionDeclarator.ParameterList.GetParameters())
            {
                _localVariableNames.Add(parameter.IdentifierName.Token.Text);
            }

            if (TryRenameFunctionDeclarator(functionDeclaration.FunctionDeclarator, out var renamedFunctionDeclarator) |
                TryRenameStatementList(functionDeclaration.Statements, out var renamedStatements))
            {
                _localVariableNames.Clear();

                renamedFunctionDeclaration = new JassFunctionDeclarationSyntax(
                    renamedFunctionDeclarator ?? functionDeclaration.FunctionDeclarator,
                    renamedStatements ?? functionDeclaration.Statements,
                    functionDeclaration.EndFunctionToken);

                return true;
            }

            _localVariableNames.Clear();

            renamedFunctionDeclaration = null;
            return false;
        }
    }
}