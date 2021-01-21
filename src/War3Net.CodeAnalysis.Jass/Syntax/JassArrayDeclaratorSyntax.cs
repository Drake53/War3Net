// ------------------------------------------------------------------------------
// <copyright file="JassArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArrayDeclaratorSyntax : IVariableDeclarator
    {
        public JassArrayDeclaratorSyntax(JassTypeSyntax type, JassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public JassTypeSyntax Type { get; init; }

        public JassIdentifierNameSyntax IdentifierName { get; init; }

        public bool Equals(IVariableDeclarator? other)
        {
            return other is JassArrayDeclaratorSyntax arrayDeclarator
                && Type.Equals(arrayDeclarator.Type)
                && IdentifierName.Equals(arrayDeclarator.IdentifierName);
        }

        public override string ToString() => $"{Type} {JassKeyword.Array} {IdentifierName}";
    }
}