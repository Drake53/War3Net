namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameGlobalDeclaration(JassGlobalDeclarationSyntax globalDeclaration, [NotNullWhen(true)] out JassGlobalDeclarationSyntax? renamedDeclaration)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => TryRenameGlobalConstantDeclaration(globalConstantDeclaration, out renamedDeclaration),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => TryRenameGlobalVariableDeclaration(globalVariableDeclaration, out renamedDeclaration),

                _ => TryRenameDummy(globalDeclaration, out renamedDeclaration),
            };
        }
    }
}