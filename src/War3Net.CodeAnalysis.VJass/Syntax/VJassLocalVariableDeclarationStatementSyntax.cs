// ------------------------------------------------------------------------------
// <copyright file="VJassLocalVariableDeclarationStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLocalVariableDeclarationStatementSyntax : IStatementSyntax
    {
        public VJassLocalVariableDeclarationStatementSyntax(
            IVariableDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public IVariableDeclaratorSyntax Declarator { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement
                && Declarator.Equals(localVariableDeclarationStatement.Declarator);
        }

        public override string ToString() => $"{VJassKeyword.Local} {Declarator}";
    }
}