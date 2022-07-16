// ------------------------------------------------------------------------------
// <copyright file="VJassEmptyParameterListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassEmptyParameterListSyntax : VJassParameterListOrEmptyParameterListSyntax
    {
        public static readonly VJassEmptyParameterListSyntax Value = new(
            new VJassSyntaxToken(VJassSyntaxKind.TakesKeyword, VJassKeyword.Takes, VJassSyntaxTriviaList.SingleSpace),
            new VJassSyntaxToken(VJassSyntaxKind.NothingKeyword, VJassKeyword.Nothing, VJassSyntaxTriviaList.SingleSpace));

        internal VJassEmptyParameterListSyntax(
            VJassSyntaxToken takesToken,
            VJassSyntaxToken nothingToken)
        {
            TakesToken = takesToken;
            NothingToken = nothingToken;
        }

        public VJassSyntaxToken TakesToken { get; }

        public VJassSyntaxToken NothingToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassEmptyParameterListSyntax;
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            NothingToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            TakesToken.ProcessTo(writer, context);
            NothingToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{TakesToken} {NothingToken}";

        public override VJassSyntaxToken GetFirstToken() => TakesToken;

        public override VJassSyntaxToken GetLastToken() => NothingToken;

        protected internal override VJassEmptyParameterListSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassEmptyParameterListSyntax(
                newToken,
                NothingToken);
        }

        protected internal override VJassEmptyParameterListSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassEmptyParameterListSyntax(
                TakesToken,
                newToken);
        }
    }
}