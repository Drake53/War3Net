using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameDeclaration(JassTopLevelDeclarationSyntax declaration, [NotNullWhen(true)] out JassTopLevelDeclarationSyntax? renamedDeclaration)
        {
            return declaration switch
            {
                JassGlobalsDeclarationSyntax globalsDeclaration => TryRenameGlobalsDeclaration(globalsDeclaration, out renamedDeclaration),
                JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration => TryRenameNativeFunctionDeclaration(nativeFunctionDeclaration, out renamedDeclaration),
                JassFunctionDeclarationSyntax functionDeclaration => TryRenameFunctionDeclaration(functionDeclaration, out renamedDeclaration),

                _ => TryRenameDummy(declaration, out renamedDeclaration),
            };
        }
    }
}