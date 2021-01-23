// ------------------------------------------------------------------------------
// <copyright file="JassGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalDeclarationSyntax : IDeclarationSyntax
    {
        public JassGlobalDeclarationSyntax(IVariableDeclarator declarator)
        {
            Declarator = declarator;
        }

        public IVariableDeclarator Declarator { get; init; }

        public bool Equals(IDeclarationSyntax? other)
        {
            return other is JassGlobalDeclarationSyntax globalDeclaration
                && Declarator.Equals(globalDeclaration.Declarator);
        }

        public override string ToString() => Declarator.ToString();
    }
}