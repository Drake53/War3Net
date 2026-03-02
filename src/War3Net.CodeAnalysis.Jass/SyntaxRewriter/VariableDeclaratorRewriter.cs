using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public abstract partial class JassSyntaxRewriter
    {
        /// <param name="variableDeclarator">The <see cref="JassVariableDeclaratorSyntax"/> to rewrite.</param>
        /// <param name="result">The rewritten <see cref="JassVariableOrArrayDeclaratorSyntax"/>, or the input <paramref name="variableDeclarator"/> if it wasn't rewritten.</param>
        /// <returns><see langword="true"/> if the <paramref name="variableDeclarator"/> was rewritten to <paramref name="result"/>, otherwise <see langword="false"/>.</returns>
        protected virtual bool RewriteVariableDeclarator(JassVariableDeclaratorSyntax variableDeclarator, out JassVariableOrArrayDeclaratorSyntax result)
        {
            if (RewriteType(variableDeclarator.Type, out var type) |
                RewriteIdentifierName(variableDeclarator.IdentifierName, out var identifierName) |
                RewriteEqualsValueClause(variableDeclarator.Value, out var value))
            {
                result = new JassVariableDeclaratorSyntax(
                    type,
                    identifierName,
                    value);

                return true;
            }

            result = variableDeclarator;
            return false;
        }
    }
}