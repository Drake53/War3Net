// ------------------------------------------------------------------------------
// <copyright file="JassEmptyDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEmptyDeclarationSyntax : IDeclarationSyntax
    {
        public static readonly JassEmptyDeclarationSyntax Value = new JassEmptyDeclarationSyntax();

        private JassEmptyDeclarationSyntax()
        {
        }

        public bool Equals(IDeclarationSyntax? other) => other is JassEmptyDeclarationSyntax;

        public override string ToString() => string.Empty;
    }
}