// ------------------------------------------------------------------------------
// <copyright file="VariableDefinitionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static LocalVariableDeclarationSyntax VariableDefinition(TypeSyntax typeNode, string variableName)
        {
            return new LocalVariableDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.LocalKeyword), 0),
                new VariableDeclarationSyntax(new VariableDefinitionSyntax(
                    typeNode,
                    new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, variableName), 0),
                    new EmptyNode(0))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }

        public static LocalVariableDeclarationSyntax VariableDefinition(TypeSyntax typeNode, string variableName, NewExpressionSyntax value)
        {
            return new LocalVariableDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.LocalKeyword), 0),
                new VariableDeclarationSyntax(new VariableDefinitionSyntax(
                    typeNode,
                    new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, variableName), 0),
                    new EqualsValueClauseSyntax(
                        new TokenNode(new SyntaxToken(SyntaxTokenType.Assignment), 0),
                        value))),
                new LineDelimiterSyntax(new EndOfLineSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NewlineSymbol), 0))));
        }
    }
}