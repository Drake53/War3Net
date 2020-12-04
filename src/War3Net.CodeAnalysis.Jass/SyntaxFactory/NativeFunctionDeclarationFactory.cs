// ------------------------------------------------------------------------------
// <copyright file="NativeFunctionDeclarationFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NativeFunctionDeclarationSyntax NativeFunctionDeclaration(FunctionDeclarationSyntax functionDeclaration, bool constant)
        {
            return constant
                ? new NativeFunctionDeclarationSyntax(Empty(), Token(SyntaxTokenType.NativeKeyword), functionDeclaration)
                : new NativeFunctionDeclarationSyntax(Token(SyntaxTokenType.ConstantKeyword), Token(SyntaxTokenType.NativeKeyword), functionDeclaration);
        }
    }
}