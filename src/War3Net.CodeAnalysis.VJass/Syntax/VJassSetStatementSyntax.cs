// ------------------------------------------------------------------------------
// <copyright file="VJassSetStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSetStatementSyntax : VJassStatementSyntax
    {
        internal VJassSetStatementSyntax(
            VJassSyntaxToken setToken,
            VJassExpressionSyntax identifier,
            VJassEqualsValueClauseSyntax value)
        {
            SetToken = setToken;
            Identifier = identifier;
            Value = value;
        }

        public VJassSyntaxToken SetToken { get; }

        public VJassExpressionSyntax Identifier { get; }

        public VJassEqualsValueClauseSyntax Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassSetStatementSyntax setStatement
                && Identifier.IsEquivalentTo(setStatement.Identifier)
                && Value.IsEquivalentTo(setStatement.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            SetToken.WriteTo(writer);
            Identifier.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            SetToken.ProcessTo(writer, context);
            Identifier.ProcessTo(writer, context);
            Value.ProcessTo(writer, context);
        }

        public override string ToString() => $"{SetToken} {Identifier} {Value}";

        public override VJassSyntaxToken GetFirstToken() => SetToken;

        public override VJassSyntaxToken GetLastToken() => Value.GetLastToken();

        protected internal override VJassSetStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassSetStatementSyntax(
                newToken,
                Identifier,
                Value);
        }

        protected internal override VJassSetStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassSetStatementSyntax(
                SetToken,
                Identifier,
                Value.ReplaceLastToken(newToken));
        }
    }
}