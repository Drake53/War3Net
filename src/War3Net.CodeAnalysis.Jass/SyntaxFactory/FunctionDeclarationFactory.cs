// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName)
        {
            return new FunctionDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.TakesKeyword), 0),
                new ParameterListReferenceSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NothingKeyword), 0)),
                new TokenNode(new SyntaxToken(SyntaxTokenType.ReturnsKeyword), 0),
                new TypeIdentifierSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NothingKeyword), 0)));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, ParameterListSyntax parameters)
        {
            return new FunctionDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.TakesKeyword), 0),
                new ParameterListReferenceSyntax(parameters),
                new TokenNode(new SyntaxToken(SyntaxTokenType.ReturnsKeyword), 0),
                new TypeIdentifierSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NothingKeyword), 0)));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, TypeSyntax returnType)
        {
            return new FunctionDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.TakesKeyword), 0),
                new ParameterListReferenceSyntax(new TokenNode(new SyntaxToken(SyntaxTokenType.NothingKeyword), 0)),
                new TokenNode(new SyntaxToken(SyntaxTokenType.ReturnsKeyword), 0),
                new TypeIdentifierSyntax(returnType));
        }

        public static FunctionDeclarationSyntax FunctionDeclaration(string functionName, ParameterListSyntax parameters, TypeSyntax returnType)
        {
            return new FunctionDeclarationSyntax(
                new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, functionName), 0),
                new TokenNode(new SyntaxToken(SyntaxTokenType.TakesKeyword), 0),
                new ParameterListReferenceSyntax(parameters),
                new TokenNode(new SyntaxToken(SyntaxTokenType.ReturnsKeyword), 0),
                new TypeIdentifierSyntax(returnType));
        }
    }
}