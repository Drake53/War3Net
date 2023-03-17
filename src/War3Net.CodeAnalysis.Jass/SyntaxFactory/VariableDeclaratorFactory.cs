// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                identifierName,
                null);
        }

        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, string name)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                ParseIdentifierName(name),
                null);
        }

        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassEqualsValueClauseSyntax? value)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                identifierName,
                value);
        }

        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, JassIdentifierNameSyntax identifierName, JassExpressionSyntax value)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                identifierName,
                EqualsValueClause(value));
        }

        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, string name, JassEqualsValueClauseSyntax? value)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                ParseIdentifierName(name),
                value);
        }

        public static JassVariableDeclaratorSyntax VariableDeclarator(JassTypeSyntax type, string name, JassExpressionSyntax value)
        {
            return new JassVariableDeclaratorSyntax(
                type,
                ParseIdentifierName(name),
                EqualsValueClause(value));
        }
    }
}