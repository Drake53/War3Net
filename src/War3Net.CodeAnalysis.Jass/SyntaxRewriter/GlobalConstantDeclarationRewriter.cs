namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="globalConstantDeclaration">The <see cref="JassGlobalConstantDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassGlobalDeclarationSyntax"/>, or the input <paramref name="globalConstantDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="globalConstantDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteGlobalConstantDeclaration(JassGlobalConstantDeclarationSyntax globalConstantDeclaration, out JassGlobalDeclarationSyntax result)
        {
            if (RewriteToken(globalConstantDeclaration.ConstantToken, out var constantToken) |
                RewriteType(globalConstantDeclaration.Type, out var type) |
                RewriteIdentifierName(globalConstantDeclaration.IdentifierName, out var identifierName) |
                RewriteEqualsValueClause(globalConstantDeclaration.Value, out var value))
            {
                result = new JassGlobalConstantDeclarationSyntax(
                    constantToken,
                    type,
                    identifierName,
                    value);

                return true;
            }

            result = globalConstantDeclaration;
            return false;
        }
    }
}