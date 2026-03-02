namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameIdentifierName(JassIdentifierNameSyntax identifierName, [NotNullWhen(true)] out JassExpressionSyntax? renamedIdentifierName)
        {
            if (TryRenameVariableIdentifierToken(identifierName.Token, out var renamedToken))
            {
                renamedIdentifierName = new JassIdentifierNameSyntax(renamedToken);
                return true;
            }

            renamedIdentifierName = null;
            return false;
        }

        private bool TryRenameFunctionIdentifierName(JassIdentifierNameSyntax identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? renamedIdentifierName)
        {
            if (TryRenameFunctionIdentifierToken(identifierName.Token, out var renamedToken))
            {
                renamedIdentifierName = new JassIdentifierNameSyntax(renamedToken);
                return true;
            }

            renamedIdentifierName = null;
            return false;
        }

        private bool TryRenameVariableIdentifierName(JassIdentifierNameSyntax identifierName, [NotNullWhen(true)] out JassIdentifierNameSyntax? renamedIdentifierName)
        {
            if (TryRenameVariableIdentifierToken(identifierName.Token, out var renamedToken))
            {
                renamedIdentifierName = new JassIdentifierNameSyntax(renamedToken);
                return true;
            }

            renamedIdentifierName = null;
            return false;
        }
    }
}