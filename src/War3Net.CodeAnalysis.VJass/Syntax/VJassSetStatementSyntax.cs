// ------------------------------------------------------------------------------
// <copyright file="VJassSetStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSetStatementSyntax : IStatementSyntax
    {
        public VJassSetStatementSyntax(
            IExpressionSyntax identifier,
            VJassEqualsValueClauseSyntax value)
        {
            Identifier = identifier;
            Value = value;
        }

        public IExpressionSyntax Identifier { get; }

        public VJassEqualsValueClauseSyntax Value { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassSetStatementSyntax setStatement
                && Identifier.Equals(setStatement.Identifier)
                && Value.Equals(setStatement.Value);
        }

        public override string ToString() => $"{VJassKeyword.Set} {Identifier} {Value}";
    }
}