// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(TypeSyntax typeNode, string variableName)
        {
            return LocalVariableDeclaration(VariableDeclaration(typeNode, variableName));
        }

        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(TypeSyntax typeNode, string variableName, EqualsValueClauseSyntax equalsValue)
        {
            return LocalVariableDeclaration(VariableDeclaration(typeNode, variableName, equalsValue));
        }

        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(TypeSyntax typeNode, string variableName, NewExpressionSyntax equalsValueExpression)
        {
            return LocalVariableDeclaration(VariableDeclaration(typeNode, variableName, equalsValueExpression));
        }

        public static LocalVariableDeclarationSyntax LocalArrayDeclaration(TypeSyntax typeNode, string arrayName)
        {
            return LocalVariableDeclaration(new VariableDeclarationSyntax(ArrayDefinition(typeNode, arrayName)));
        }

        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(VariableDeclarationSyntax variableDeclaration)
        {
            return LocalVariableDeclaration(variableDeclaration, Newlines());
        }

        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(VariableDeclarationSyntax variableDeclaration, LineDelimiterSyntax lineDelimiter)
        {
            return new LocalVariableDeclarationSyntax(
                Token(SyntaxTokenType.LocalKeyword),
                variableDeclaration,
                lineDelimiter);
        }
    }
}