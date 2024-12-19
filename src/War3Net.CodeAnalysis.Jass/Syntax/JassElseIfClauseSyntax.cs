// ------------------------------------------------------------------------------
// <copyright file="JassElseIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElseIfClauseSyntax : IEquatable<JassElseIfClauseSyntax>, IJassSyntaxToken
    {
        public JassElseIfClauseSyntax(IExpressionSyntax condition, JassStatementListSyntax body)
        {
            Condition = condition;
            Body = body;
        }

        public IExpressionSyntax Condition { get; init; }

        public JassStatementListSyntax Body { get; init; }

        public bool Equals(JassElseIfClauseSyntax? other)
        {
            return other is not null
                && Condition.Equals(other.Condition)
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{JassKeyword.ElseIf} {Condition} {JassKeyword.Then} [{Body.Statements.Length}]";
    }
}