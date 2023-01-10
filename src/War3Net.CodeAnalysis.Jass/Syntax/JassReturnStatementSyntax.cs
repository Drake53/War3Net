// ------------------------------------------------------------------------------
// <copyright file="JassReturnStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassReturnStatementSyntax : JassStatementSyntax
    {
        public static readonly JassReturnStatementSyntax Empty = new(
            new JassSyntaxToken(JassSyntaxKind.ReturnKeyword, JassKeyword.Return, JassSyntaxTriviaList.Empty),
            null);

        internal JassReturnStatementSyntax(
            JassSyntaxToken returnToken,
            JassExpressionSyntax? value)
        {
            ReturnToken = returnToken;
            Value = value;
        }

        public JassSyntaxToken ReturnToken { get; }

        public JassExpressionSyntax? Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassReturnStatementSyntax returnStatement
                && Value.NullableEquivalentTo(returnStatement.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnToken.WriteTo(writer);
            Value?.WriteTo(writer);
        }

        public override string ToString() => $"{ReturnToken}{Value.OptionalPrefixed()}";

        public override JassSyntaxToken GetFirstToken() => ReturnToken;

        public override JassSyntaxToken GetLastToken() => Value?.GetLastToken() ?? ReturnToken;

        protected internal override JassReturnStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassReturnStatementSyntax(
                newToken,
                Value);
        }

        protected internal override JassReturnStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (Value is not null)
            {
                return new JassReturnStatementSyntax(
                    ReturnToken,
                    Value.ReplaceLastToken(newToken));
            }

            return new JassReturnStatementSyntax(
                newToken,
                null);
        }
    }
}