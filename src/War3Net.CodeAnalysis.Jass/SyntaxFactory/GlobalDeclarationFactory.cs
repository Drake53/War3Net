// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static GlobalDeclarationSyntax GlobalDeclaration(TypeSyntax typeNode, string variableName)
        {
            return GlobalDeclaration(VariableDeclaration(typeNode, variableName));
        }

        public static GlobalDeclarationSyntax GlobalDeclaration(TypeSyntax typeNode, string variableName, EqualsValueClauseSyntax equalsValue)
        {
            return GlobalDeclaration(VariableDeclaration(typeNode, variableName, equalsValue));
        }

        public static GlobalDeclarationSyntax GlobalDeclaration(TypeSyntax typeNode, string variableName, NewExpressionSyntax equalsValueExpression)
        {
            return GlobalDeclaration(VariableDeclaration(typeNode, variableName, equalsValueExpression));
        }

        public static GlobalDeclarationSyntax GlobalArrayDeclaration(TypeSyntax typeNode, string arrayName)
        {
            return GlobalDeclaration(new VariableDeclarationSyntax(ArrayDefinition(typeNode, arrayName)));
        }

        public static GlobalDeclarationSyntax GlobalDeclaration(VariableDeclarationSyntax variableDeclaration)
        {
            return GlobalDeclaration(variableDeclaration, Newlines());
        }

        public static GlobalDeclarationSyntax GlobalDeclaration(VariableDeclarationSyntax variableDeclaration, LineDelimiterSyntax lineDelimiter)
        {
            return new GlobalDeclarationSyntax(
                new GlobalVariableDeclarationSyntax(
                    variableDeclaration,
                    lineDelimiter));
        }
    }
}