// ------------------------------------------------------------------------------
// <copyright file="VJassHookDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassHookDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassHookDeclarationSyntax(
            VJassIdentifierNameSyntax hookedFunction,
            VJassIdentifierNameSyntax identifierName)
        {
            HookedFunction = hookedFunction;
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax HookedFunction { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassHookDeclarationSyntax hookDeclaration
                && HookedFunction.Equals(hookDeclaration.HookedFunction)
                && IdentifierName.Equals(hookDeclaration.IdentifierName);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassHookDeclarationSyntax hookDeclaration
                && HookedFunction.Equals(hookDeclaration.HookedFunction)
                && IdentifierName.Equals(hookDeclaration.IdentifierName);
        }

        public override string ToString() => $"{VJassKeyword.Hook} {HookedFunction} {IdentifierName}";
    }
}