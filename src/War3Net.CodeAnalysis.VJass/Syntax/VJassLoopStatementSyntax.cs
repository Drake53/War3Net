// ------------------------------------------------------------------------------
// <copyright file="VJassLoopStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLoopStatementSyntax : IStatementSyntax
    {
        public VJassLoopStatementSyntax(
            VJassStatementListSyntax body)
        {
            Body = body;
        }

        public VJassStatementListSyntax Body { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassLoopStatementSyntax loopStatement
                && Body.Equals(loopStatement.Body);
        }

        public override string ToString() => $"{VJassKeyword.Loop} [{Body.Statements.Length}]";
    }
}