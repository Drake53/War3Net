// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

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