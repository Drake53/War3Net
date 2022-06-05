// ------------------------------------------------------------------------------
// <copyright file="VJassScopeDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopeDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassScopeDeclarationSyntax(
            VJassIdentifierNameSyntax identifierName,
            VJassIdentifierNameSyntax? initializer,
            VJassScopedDeclarationListSyntax declarations)
        {
            Initializer = identifierName;
            Initializer = initializer;
            Declarations = declarations;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassIdentifierNameSyntax? Initializer { get; }

        public VJassScopedDeclarationListSyntax Declarations { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassScopeDeclarationSyntax scopeDeclaration
                && IdentifierName.Equals(scopeDeclaration.IdentifierName)
                && Initializer.NullableEquals(scopeDeclaration.Initializer)
                && Declarations.Equals(scopeDeclaration.Declarations);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassScopeDeclarationSyntax scopeDeclaration
                && IdentifierName.Equals(scopeDeclaration.IdentifierName)
                && Initializer.NullableEquals(scopeDeclaration.Initializer)
                && Declarations.Equals(scopeDeclaration.Declarations);
        }

        public override string ToString() => Initializer is null
            ? $"{VJassKeyword.Scope} {IdentifierName} [{Declarations.Declarations.Length}]"
            : $"{VJassKeyword.Scope} {IdentifierName} {VJassKeyword.Initializer} {Initializer} [{Declarations.Declarations.Length}]";
    }
}