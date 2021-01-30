// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassGlobalDeclarationSyntax GlobalDeclaration(JassTypeSyntax type, string name)
        {
            return new JassGlobalDeclarationSyntax(
                new JassVariableDeclaratorSyntax(
                    type,
                    ParseIdentifierName(name),
                    null));
        }

        public static JassGlobalDeclarationSyntax GlobalDeclaration(JassTypeSyntax type, string name, IExpressionSyntax value)
        {
            return new JassGlobalDeclarationSyntax(
                new JassVariableDeclaratorSyntax(
                    type,
                    ParseIdentifierName(name),
                    new JassEqualsValueClauseSyntax(value)));
        }

        public static JassGlobalDeclarationSyntax GlobalArrayDeclaration(JassTypeSyntax type, string name)
        {
            return new JassGlobalDeclarationSyntax(
                new JassArrayDeclaratorSyntax(
                    type, ParseIdentifierName(name)));
        }
    }
}