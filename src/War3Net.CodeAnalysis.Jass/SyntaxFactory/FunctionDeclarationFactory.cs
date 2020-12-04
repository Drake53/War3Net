// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName)
        {
            return FunctionDeclaration(functionName, new ParameterListReferenceSyntax(Token(SyntaxTokenType.NothingKeyword)), new TypeIdentifierSyntax(Token(SyntaxTokenType.NothingKeyword)));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, ParameterListSyntax parameters)
        {
            return FunctionDeclaration(functionName, new ParameterListReferenceSyntax(parameters), new TypeIdentifierSyntax(Token(SyntaxTokenType.NothingKeyword)));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, TypeSyntax returnType)
        {
            return FunctionDeclaration(functionName, new ParameterListReferenceSyntax(Token(SyntaxTokenType.NothingKeyword)), new TypeIdentifierSyntax(returnType));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, ParameterListSyntax parameters, TypeSyntax returnType)
        {
            return FunctionDeclaration(functionName, new ParameterListReferenceSyntax(parameters), new TypeIdentifierSyntax(returnType));
        }

        private static FunctionDeclarationSyntax FunctionDeclaration(string functionName, ParameterListReferenceSyntax parameterList, TypeIdentifierSyntax returnType)
        {
            return new FunctionDeclarationSyntax(
                Token(SyntaxTokenType.AlphanumericIdentifier, functionName),
                Token(SyntaxTokenType.TakesKeyword),
                parameterList,
                Token(SyntaxTokenType.ReturnsKeyword),
                returnType);
        }
    }
}