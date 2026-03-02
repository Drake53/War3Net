namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameGlobalVariableDeclaration(JassGlobalVariableDeclarationSyntax globalVariableDeclaration, [NotNullWhen(true)] out JassGlobalDeclarationSyntax? renamedGlobalVariableDeclaration)
        {
            if (TryRenameVariableOrArrayDeclarator(globalVariableDeclaration.Declarator, out var renamedDeclarator))
            {
                renamedGlobalVariableDeclaration = new JassGlobalVariableDeclarationSyntax(renamedDeclarator);
                return true;
            }

            renamedGlobalVariableDeclaration = null;
            return false;
        }
    }
}