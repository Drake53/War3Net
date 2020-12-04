// ------------------------------------------------------------------------------
// <copyright file="VariableDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax typeNode, string variableName)
        {
            return new VariableDeclarationSyntax(new VariableDefinitionSyntax(
                typeNode,
                Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                Empty()));
        }

        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax typeNode, string variableName, EqualsValueClauseSyntax equalsValue)
        {
            return new VariableDeclarationSyntax(new VariableDefinitionSyntax(
                typeNode,
                Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                equalsValue));
        }

        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax typeNode, string variableName, NewExpressionSyntax equalsValueExpression)
        {
            return new VariableDeclarationSyntax(new VariableDefinitionSyntax(
                typeNode,
                Token(SyntaxTokenType.AlphanumericIdentifier, variableName),
                EqualsValueClause(equalsValueExpression)));
        }

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

        public static LocalVariableDeclarationSyntax LocalVariableDeclaration(VariableDeclarationSyntax variableDeclaration, LineDelimiterSyntax? lineDelimiter = null)
        {
            return new LocalVariableDeclarationSyntax(Token(SyntaxTokenType.LocalKeyword), variableDeclaration, lineDelimiter ?? Newlines());
        }
    }
}