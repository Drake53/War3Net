// ------------------------------------------------------------------------------
// <copyright file="JassExitStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassExitStatementSyntax : IStatementSyntax, ICustomScriptAction
    {
        public JassExitStatementSyntax(IExpressionSyntax condition)
        {
            Condition = condition;
        }

        public IExpressionSyntax Condition { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassExitStatementSyntax exitStatement
                && Condition.Equals(exitStatement.Condition);
        }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassExitStatementSyntax exitStatement
                && Condition.Equals(exitStatement.Condition);
        }

        public override string ToString() => $"{JassKeyword.ExitWhen} {Condition}";
    }
}