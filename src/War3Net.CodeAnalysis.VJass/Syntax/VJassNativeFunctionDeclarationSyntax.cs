// ------------------------------------------------------------------------------
// <copyright file="VJassNativeFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassNativeFunctionDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassNativeFunctionDeclarationSyntax(
            VJassFunctionDeclaratorSyntax functionDeclarator)
        {
            FunctionDeclarator = functionDeclarator;
        }

        public VJassFunctionDeclaratorSyntax FunctionDeclarator { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassNativeFunctionDeclarationSyntax nativeFunctionDeclaration
                && FunctionDeclarator.Equals(nativeFunctionDeclaration.FunctionDeclarator);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassNativeFunctionDeclarationSyntax nativeFunctionDeclaration
                && FunctionDeclarator.Equals(nativeFunctionDeclaration.FunctionDeclarator);
        }

        public override string ToString() => $"{VJassKeyword.Native} {FunctionDeclarator}";
    }
}