namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="globalDeclaration">The <see cref="JassGlobalDeclarationSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassGlobalDeclarationSyntax"/>, or the input <paramref name="globalDeclaration"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="globalDeclaration"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteGlobalDeclaration(JassGlobalDeclarationSyntax globalDeclaration, out JassGlobalDeclarationSyntax result)
        {
            return globalDeclaration switch
            {
                JassGlobalConstantDeclarationSyntax globalConstantDeclaration => RewriteGlobalConstantDeclaration(globalConstantDeclaration, out result),
                JassGlobalVariableDeclarationSyntax globalVariableDeclaration => RewriteGlobalVariableDeclaration(globalVariableDeclaration, out result),
            };
        }
    }
}