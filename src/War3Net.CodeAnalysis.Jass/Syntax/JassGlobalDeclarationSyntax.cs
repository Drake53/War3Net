// ------------------------------------------------------------------------------
// <copyright file="JassGlobalDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalDeclarationSyntax : IEquatable<JassGlobalDeclarationSyntax>
    {
        public JassGlobalDeclarationSyntax(IVariableDeclarator declarator)
        {
            Declarator = declarator;
        }

        public IVariableDeclarator Declarator { get; init; }

        public bool Equals(JassGlobalDeclarationSyntax? other)
        {
            return other is not null
                && Declarator.Equals(other.Declarator);
        }

        public override string ToString() => Declarator.ToString();
    }
}