// ------------------------------------------------------------------------------
// <copyright file="JassFunctionDeclarationBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassFunctionDeclarationBuilder : JassStatementListSyntaxBuilder
    {
        private readonly JassFunctionDeclaratorSyntax _functionDeclarator;

        public JassFunctionDeclarationBuilder(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            _functionDeclarator = functionDeclarator;
        }

        public JassFunctionDeclarationSyntax ToFunctionDeclaration(JassSyntaxToken endFunctionToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(endFunctionToken, JassSyntaxKind.EndFunctionKeyword);

            return new JassFunctionDeclarationSyntax(
                _functionDeclarator,
                BuildStatementList(),
                endFunctionToken.PrependLeadingTrivia(BuildTriviaList()));
        }
    }
}