// ------------------------------------------------------------------------------
// <copyright file="VJassReturnStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassReturnStatementSyntax : IStatementSyntax
    {
        public static readonly VJassReturnStatementSyntax Empty = new(null);

        public VJassReturnStatementSyntax(IExpressionSyntax? value)
        {
            Value = value;
        }

        public IExpressionSyntax? Value { get; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is VJassReturnStatementSyntax returnStatement
                && Value.NullableEquals(returnStatement.Value);
        }

        public override string ToString() => Value is null ? VJassKeyword.Return : $"{VJassKeyword.Return} {Value}";
    }
}