using System.Diagnostics.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameFunctionIdentifierToken(JassSyntaxToken token, [NotNullWhen(true)] out JassSyntaxToken? renamedToken)
        {
            if (_functionDeclarationRenames.TryGetValue(token.Text, out var newName))
            {
                renamedToken = new JassSyntaxToken(
                    token.LeadingTrivia,
                    token.SyntaxKind,
                    newName,
                    token.TrailingTrivia);

                return true;
            }

            renamedToken = null;
            return false;
        }

        private bool TryRenameVariableIdentifierToken(JassSyntaxToken token, [NotNullWhen(true)] out JassSyntaxToken? renamedToken)
        {
            if (_localVariableNames.Contains(token.Text))
            {
                renamedToken = null;
                return false;
            }

            if (_globalVariableRenames.TryGetValue(token.Text, out var newName))
            {
                renamedToken = new JassSyntaxToken(
                    token.LeadingTrivia,
                    token.SyntaxKind,
                    newName,
                    token.TrailingTrivia);

                return true;
            }

            renamedToken = null;
            return false;
        }
    }
}