// ------------------------------------------------------------------------------
// <copyright file="JassDebugStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassDebugStatementSyntax : IStatementSyntax, IJassSyntaxToken
    {
        public JassDebugStatementSyntax(IStatementSyntax statement)
        {
            Statement = statement;
        }

        public IStatementSyntax Statement { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassDebugStatementSyntax debugStatement
                && Statement.Equals(debugStatement.Statement);
        }

        public override string ToString() => $"{JassKeyword.Debug} {Statement}";
    }
}