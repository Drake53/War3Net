#pragma warning disable SA1649 // File name should match first type name

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NativeFunctionDeclarationSyntax NativeFunctionDeclaration(FunctionDeclarationSyntax functionDeclaration, bool constant)
        {
            return constant
                ? new NativeFunctionDeclarationSyntax(
                    new EmptyNode(0),
                    new TokenNode(new SyntaxToken(SyntaxTokenType.NativeKeyword), 0),
                    functionDeclaration)
                : new NativeFunctionDeclarationSyntax(
                    new TokenNode(new SyntaxToken(SyntaxTokenType.ConstantKeyword), 0),
                    new TokenNode(new SyntaxToken(SyntaxTokenType.NativeKeyword), 0),
                    functionDeclaration);
        }
    }
}