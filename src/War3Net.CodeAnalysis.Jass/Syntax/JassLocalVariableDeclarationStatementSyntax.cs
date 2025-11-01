// ------------------------------------------------------------------------------
// <copyright file="JassLocalVariableDeclarationStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLocalVariableDeclarationStatementSyntax : IStatementSyntax, IStatementLineSyntax, IJassSyntaxToken
    {
        public JassLocalVariableDeclarationStatementSyntax(IVariableDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public IVariableDeclaratorSyntax Declarator { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement
                && Declarator.Equals(localVariableDeclarationStatement.Declarator);
        }

        public bool Equals(IStatementLineSyntax? other)
        {
            return other is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement
                && Declarator.Equals(localVariableDeclarationStatement.Declarator);
        }

        public override string ToString() => $"{JassKeyword.Local} {Declarator}";
    }
}