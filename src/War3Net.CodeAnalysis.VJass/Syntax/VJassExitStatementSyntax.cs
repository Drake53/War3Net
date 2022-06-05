// ------------------------------------------------------------------------------
// <copyright file="VJassExitStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassExitStatementSyntax : IStatementSyntax
    {
        public VJassExitStatementSyntax(IExpressionSyntax condition)
        {
            Condition = condition;
        }

        public IExpressionSyntax Condition { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassExitStatementSyntax exitStatement
                && Condition.Equals(exitStatement.Condition);
        }

        public override string ToString() => $"{VJassKeyword.ExitWhen} {Condition}";
    }
}