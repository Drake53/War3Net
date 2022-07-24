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
            VJassSyntaxToken dotToken,
            VJassIdentifierNameSyntax memberName)
        {
            Expression = expression;
            DotToken = dotToken;
            MemberName = memberName;
        }

        public VJassExpressionSyntax Expression { get; }

        public VJassSyntaxToken DotToken { get; }

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
            DotToken.WriteTo(writer);
            MemberName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Expression.ProcessTo(writer, context);
            DotToken.ProcessTo(writer, context);
            MemberName.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Expression}{DotToken}{MemberName}";

        public override VJassSyntaxToken GetFirstToken() => Expression.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => MemberName.GetLastToken();

        protected internal override VJassMemberAccessExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberAccessExpressionSyntax(
                Expression.ReplaceFirstToken(newToken),
                DotToken,
                MemberName);
        }

        protected internal override VJassMemberAccessExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberAccessExpressionSyntax(
                Expression,
                DotToken,
                MemberName.ReplaceLastToken(newToken));
        }
    }
}