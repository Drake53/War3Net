namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameVariableDeclarator(JassVariableDeclaratorSyntax variableDeclarator, [NotNullWhen(true)] out JassVariableOrArrayDeclaratorSyntax? renamedVariableDeclarator)
        {
            if (TryRenameVariableIdentifierName(variableDeclarator.IdentifierName, out var renamedIdentifierName) |
                TryRenameEqualsValueClause(variableDeclarator.Value, out var renamedValue))
            {
                renamedVariableDeclarator = new JassVariableDeclaratorSyntax(
                    variableDeclarator.Type,
                    renamedIdentifierName ?? variableDeclarator.IdentifierName,
                    renamedValue ?? variableDeclarator.Value);

                return true;
            }

            renamedVariableDeclarator = null;
            return false;
        }
    }
}