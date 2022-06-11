// ------------------------------------------------------------------------------
// <copyright file="VJassCallStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassCallStatementSyntax : VJassStatementSyntax
    {
        internal VJassCallStatementSyntax(
            VJassSyntaxToken callToken,
            VJassExpressionSyntax expression)
        {
            CallToken = callToken;
            Expression = expression;
        }

        public VJassSyntaxToken CallToken { get; }

        public VJassExpressionSyntax Expression { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassCallStatementSyntax callStatement
                && Expression.IsEquivalentTo(callStatement.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            CallToken.WriteTo(writer);
            Expression.WriteTo(writer);
        }

        public override string ToString() => $"{CallToken} {Expression}";

        public override VJassSyntaxToken GetFirstToken() => CallToken;

        public override VJassSyntaxToken GetLastToken() => Expression.GetLastToken();

        protected internal override VJassCallStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassCallStatementSyntax(
                newToken,
                Expression);
        }

        protected internal override VJassCallStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassCallStatementSyntax(
                CallToken,
                Expression.ReplaceLastToken(newToken));
        }
    }
}