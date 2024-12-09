// ------------------------------------------------------------------------------
// <copyright file="JassLoopStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLoopStatementSyntax : IStatementSyntax, IJassSyntaxToken
    {
        public JassLoopStatementSyntax(JassStatementListSyntax body)
        {
            Body = body;
        }

        public JassStatementListSyntax Body { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassLoopStatementSyntax loopStatement
                && Body.Equals(loopStatement.Body);
        }

        public override string ToString() => $"{JassKeyword.Loop} [{Body.Statements.Length}]";
    }
}