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
        internal VJassArgumentListSyntax(
            VJassSyntaxToken openParenthesisToken,
            SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> argumentList,
            VJassSyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            ArgumentList = argumentList;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public VJassSyntaxToken OpenParenthesisToken { get; }

        public SeparatedSyntaxList<VJassExpressionSyntax, VJassSyntaxToken> ArgumentList { get; }

        public VJassSyntaxToken CloseParenthesisToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassArgumentListSyntax argumentList
                && ArgumentList.Items.IsEquivalentTo(argumentList.ArgumentList.Items);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenParenthesisToken.WriteTo(writer);
            ArgumentList.WriteTo(writer);
            CloseParenthesisToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            OpenParenthesisToken.ProcessTo(writer, context);
            ArgumentList.ProcessTo(writer, context);
            CloseParenthesisToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{OpenParenthesisToken}{ArgumentList}{CloseParenthesisToken}";

        public override VJassSyntaxToken GetFirstToken() => OpenParenthesisToken;

        public override VJassSyntaxToken GetLastToken() => CloseParenthesisToken;

        protected internal override VJassArgumentListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                newToken,
                ArgumentList,
                CloseParenthesisToken);
        }

        protected internal override VJassArgumentListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassArgumentListSyntax(
                OpenParenthesisToken,
                ArgumentList,
                newToken);
        }
    }
}