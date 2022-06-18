// ------------------------------------------------------------------------------
// <copyright file="VJassArgumentListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassArgumentListSyntax : VJassSyntaxNode
    {
        public static readonly VJassArgumentListSyntax Empty = new(
            VJassSyntaxFactory.Token(VJassSyntaxKind.OpenParenToken),
            SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken>.Empty,
            VJassSyntaxFactory.Token(VJassSyntaxKind.CloseParenToken));

        internal VJassArgumentListSyntax(
            VJassSyntaxToken leftParenthesisToken,
            SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> argumentList,
            VJassSyntaxToken rightParenthesisToken)
        {
            LeftParenthesisToken = leftParenthesisToken;
            ArgumentList = argumentList;
            RightParenthesisToken = rightParenthesisToken;
        }

        public VJassSyntaxToken LeftParenthesisToken { get; }

        public SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> ArgumentList { get; }

        public VJassSyntaxToken RightParenthesisToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassArgumentListSyntax argumentList
                && ArgumentList.Items.IsEquivalentTo(argumentList.ArgumentList.Items);
        }

        public override void WriteTo(TextWriter writer)
        {
            LeftParenthesisToken.WriteTo(writer);
            ArgumentList.WriteTo(writer);
            RightParenthesisToken.WriteTo(writer);
        }

        public override string ToString() => $"{LeftParenthesisToken}{ArgumentList}{RightParenthesisToken}";

        public override VJassSyntaxToken GetFirstToken() => LeftParenthesisToken;

        public override VJassSyntaxToken GetLastToken() => RightParenthesisToken;

        protected internal override VJassArgumentListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                newToken,
                ArgumentList,
                RightParenthesisToken);
        }

        protected internal override VJassArgumentListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                LeftParenthesisToken,
                ArgumentList,
                newToken);
        }
    }
}