using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameNativeFunctionDeclaration(JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration, [NotNullWhen(true)] out JassTopLevelDeclarationSyntax? renamedNativeFunctionDeclaration)
        {
            if (TryRenameFunctionIdentifierName(nativeFunctionDeclaration.IdentifierName, out var renamedIdentifierName))
            {
                renamedNativeFunctionDeclaration = new JassNativeFunctionDeclarationSyntax(
                    nativeFunctionDeclaration.ConstantToken,
                    nativeFunctionDeclaration.NativeToken,
                    renamedIdentifierName,
                    nativeFunctionDeclaration.ParameterList,
                    nativeFunctionDeclaration.ReturnClause);

                return true;
            }

            renamedNativeFunctionDeclaration = null;
            return false;
        }
    }
}