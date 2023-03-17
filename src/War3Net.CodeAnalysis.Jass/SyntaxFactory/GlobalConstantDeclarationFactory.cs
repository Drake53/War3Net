// ------------------------------------------------------------------------------
// <copyright file="GlobalConstantDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassGlobalConstantDeclarationSyntax GlobalConstantDeclaration(JassTypeSyntax type, string name, JassExpressionSyntax value)
        {
            return new JassGlobalConstantDeclarationSyntax(
                Token(JassSyntaxKind.ConstantKeyword),
                type,
                ParseIdentifierName(name),
                EqualsValueClause(value));
        }
    }
}