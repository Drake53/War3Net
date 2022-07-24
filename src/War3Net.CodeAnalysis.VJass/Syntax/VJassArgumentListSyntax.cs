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
            new VJassSyntaxToken(VJassSyntaxKind.OpenParenToken, VJassSymbol.OpenParen, VJassSyntaxTriviaList.Empty),
            SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken>.Empty,
            new VJassSyntaxToken(VJassSyntaxKind.CloseParenToken, VJassSymbol.CloseParen, VJassSyntaxTriviaList.Empty));

        internal VJassArgumentListSyntax(
            VJassSyntaxToken openParenToken,
            SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> argumentList,
            VJassSyntaxToken closeParenToken)
        {
            OpenParenToken = openParenToken;
            ArgumentList = argumentList;
            CloseParenToken = closeParenToken;
        }

        public VJassSyntaxToken OpenParenToken { get; }

        public SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> ArgumentList { get; }

        public VJassSyntaxToken CloseParenToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassArgumentListSyntax argumentList
                && ArgumentList.Items.IsEquivalentTo(argumentList.ArgumentList.Items);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenToken.WriteTo(writer);
            ArgumentList.WriteTo(writer);
            CloseParenToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            OpenParenToken.ProcessTo(writer, context);
            ArgumentList.ProcessTo(writer, context);
            CloseParenToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{OpenParenToken}{ArgumentList}{CloseParenToken}";

        public override VJassSyntaxToken GetFirstToken() => OpenParenToken;

        public override VJassSyntaxToken GetLastToken() => CloseParenToken;

        protected internal override VJassArgumentListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                newToken,
                ArgumentList,
                CloseParenToken);
        }

        protected internal override VJassArgumentListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                OpenParenToken,
                ArgumentList,
                newToken);
        }
    }
}