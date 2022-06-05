// ------------------------------------------------------------------------------
// <copyright file="VJassCallStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassCallStatementSyntax : IStatementSyntax
    {
        public VJassCallStatementSyntax(IExpressionSyntax expression)
        {
            Expression = expression;
        }

        public IExpressionSyntax Expression { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassCallStatementSyntax callStatement
                && Expression.Equals(callStatement.Expression);
        }

        public override string ToString() => $"{VJassKeyword.Call} {Expression}";
    }
}