// ------------------------------------------------------------------------------
// <copyright file="VJassStatementIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStatementIfClauseSyntax : IEquatable<VJassStatementIfClauseSyntax>
    {
        public VJassStatementIfClauseSyntax(
            IExpressionSyntax condition,
            VJassStatementListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; }

        public VJassStatementListSyntax Body { get; }

        public bool Equals(VJassStatementIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.If} {Condition} {VJassKeyword.Then} [{Body.Statements.Length}]";
    }
}