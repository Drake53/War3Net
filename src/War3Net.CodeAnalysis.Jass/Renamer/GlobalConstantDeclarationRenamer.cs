using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax globalConstantDeclaration, [NotNullWhen(true)] out JassGlobalDeclarationSyntax? renamedGlobalConstantDeclaration)
        {
            if (TryRenameVariableIdentifierName(globalConstantDeclaration.IdentifierName, out var renamedIdentifierName) |
                TryRenameEqualsValueClause(globalConstantDeclaration.Value, out var renamedValue))
            {
                renamedGlobalConstantDeclaration = new JassGlobalConstantDeclarationSyntax(
                    globalConstantDeclaration.ConstantToken,
                    globalConstantDeclaration.Type,
                    renamedIdentifierName ?? globalConstantDeclaration.IdentifierName,
                    renamedValue ?? globalConstantDeclaration.Value);

                return false;
            }

            renamedGlobalConstantDeclaration = null;
            return false;
        }
    }
}