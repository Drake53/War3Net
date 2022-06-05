// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalDeclarationSyntax : IGlobalDeclarationSyntax
    {
        public VJassGlobalDeclarationSyntax(IVariableDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public IVariableDeclaratorSyntax Declarator { get; }

        public bool Equals(IGlobalDeclarationSyntax? other)
        {
            return other is VJassGlobalDeclarationSyntax globalDeclaration
                && Declarator.Equals(globalDeclaration.Declarator);
        }

        public override string ToString() => Declarator.ToString();
    }
}