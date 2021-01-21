// ------------------------------------------------------------------------------
// <copyright file="JassReturnStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassReturnStatementSyntax : IStatementSyntax, ICustomScriptAction
    {
        public JassReturnStatementSyntax(IExpressionSyntax? value)
        {
            Value = value;
        }

        public IExpressionSyntax? Value { get; init; }

        public bool Equals(IStatementSyntax? other)
        {
            return other is JassReturnStatementSyntax returnStatement
                && Value.NullableEquals(returnStatement.Value);
        }

        public bool Equals(ICustomScriptAction? other)
        {
            return other is JassReturnStatementSyntax returnStatement
                && Value.NullableEquals(returnStatement.Value);
        }

        public override string ToString() => Value is null ? JassKeyword.Return : $"{JassKeyword.Return} {Value}";
    }
}