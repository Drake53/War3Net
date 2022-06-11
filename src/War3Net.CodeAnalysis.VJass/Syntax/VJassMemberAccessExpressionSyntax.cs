// ------------------------------------------------------------------------------
// <copyright file="VJassMemberAccessExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMemberAccessExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassMemberAccessExpressionSyntax(
            VJassExpressionSyntax expression,
            VJassSyntaxToken fullStopToken,
            VJassIdentifierNameSyntax memberName)
        {
            Expression = expression;
            FullStopToken = fullStopToken;
            MemberName = memberName;
        }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken FullStopToken { get; }

        public VJassIdentifierNameSyntax MemberName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMemberAccessExpressionSyntax memberAccessExpression
                && Expression.IsEquivalentTo(memberAccessExpression.Expression)
                && MemberName.IsEquivalentTo(memberAccessExpression.MemberName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Expression.WriteTo(writer);
            FullStopToken.WriteTo(writer);
            MemberName.WriteTo(writer);
        }

        public override string ToString() => $"{Expression}{FullStopToken}{MemberName}";

        public override VJassSyntaxToken GetFirstToken() => Expression.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => MemberName.GetLastToken();

        protected internal override VJassMemberAccessExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberAccessExpressionSyntax(
                Expression.ReplaceFirstToken(newToken),
                FullStopToken,
                MemberName);
        }

        protected internal override VJassMemberAccessExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberAccessExpressionSyntax(
                Expression,
                FullStopToken,
                MemberName.ReplaceLastToken(newToken));
        }
    }
}