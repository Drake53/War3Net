// ------------------------------------------------------------------------------
// <copyright file="IfClauseDeclaratorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassIfClauseDeclaratorSyntax IfClauseDeclarator(JassExpressionSyntax condition)
        {
            return new JassIfClauseDeclaratorSyntax(
                Token(JassSyntaxKind.IfKeyword),
                condition,
                Token(JassSyntaxKind.ThenKeyword));
        }

        public static JassIfClauseDeclaratorSyntax IfClauseDeclarator(JassSyntaxToken ifToken, JassExpressionSyntax condition, JassSyntaxToken thenToken)
        {
            ThrowHelper.ThrowIfInvalidToken(ifToken, JassSyntaxKind.IfKeyword);
            ThrowHelper.ThrowIfInvalidToken(thenToken, JassSyntaxKind.ThenKeyword);

            return new JassIfClauseDeclaratorSyntax(
                ifToken,
                condition,
                thenToken);
        }
    }
}