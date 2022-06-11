// ------------------------------------------------------------------------------
// <copyright file="VJassReturnStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassReturnStatementSyntax : VJassStatementSyntax
    {
        internal VJassReturnStatementSyntax(
            VJassSyntaxToken returnToken,
            VJassExpressionSyntax? value)
        {
            ReturnToken = returnToken;
            Value = value;
        }

        public VJassSyntaxToken ReturnToken { get; }

        public VJassExpressionSyntax? Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassReturnStatementSyntax returnStatement
                && Value.NullableEquals(returnStatement.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnToken.WriteTo(writer);
            Value?.WriteTo(writer);
        }

        public override string ToString() => $"{ReturnToken}{Value.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => ReturnToken;

        public override VJassSyntaxToken GetLastToken() => Value?.GetLastToken() ?? ReturnToken;

        protected internal override VJassReturnStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassReturnStatementSyntax(
                newToken,
                Value);
        }

        protected internal override VJassReturnStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (Value is not null)
            {
                return new VJassReturnStatementSyntax(
                    ReturnToken,
                    Value.ReplaceLastToken(newToken));
            }

            return new VJassReturnStatementSyntax(
                newToken,
                null);
        }
    }
}