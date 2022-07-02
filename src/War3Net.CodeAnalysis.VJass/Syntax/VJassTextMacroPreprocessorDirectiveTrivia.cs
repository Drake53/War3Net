// ------------------------------------------------------------------------------
// <copyright file="VJassTextMacroPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTextMacroPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        internal VJassTextMacroPreprocessorDirectiveTrivia(
            VJassSyntaxToken preprocessorDirectiveStartToken,
            VJassSyntaxToken textMacroToken,
            VJassIdentifierNameSyntax identifierName,
            VJassTextMacroParameterListSyntax? parameterList,
            string body,
            VJassSyntaxToken preprocessorDirectiveEndToken,
            VJassSyntaxToken endTextMacroToken)
        {
            PreprocessorDirectiveStartToken = preprocessorDirectiveStartToken;
            TextMacroToken = textMacroToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            Body = body;
            PreprocessorDirectiveEndToken = preprocessorDirectiveEndToken;
            EndTextMacroToken = endTextMacroToken;
        }

        public VJassSyntaxToken PreprocessorDirectiveStartToken { get; }

        public VJassSyntaxToken TextMacroToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassTextMacroParameterListSyntax? ParameterList { get; }

        public string Body { get; }

        public VJassSyntaxToken PreprocessorDirectiveEndToken { get; }

        public VJassSyntaxToken EndTextMacroToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTextMacroPreprocessorDirectiveTrivia textMacroPreprocessorDirectiveTrivia
                && IdentifierName.IsEquivalentTo(textMacroPreprocessorDirectiveTrivia.IdentifierName)
                && ParameterList.NullableEquivalentTo(textMacroPreprocessorDirectiveTrivia.ParameterList)
                && string.Equals(Body, textMacroPreprocessorDirectiveTrivia.Body, StringComparison.Ordinal);
        }

        public override void WriteTo(TextWriter writer)
        {
            PreprocessorDirectiveStartToken.WriteTo(writer);
            TextMacroToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ParameterList?.WriteTo(writer);
            writer.Write(Body);
            PreprocessorDirectiveEndToken.WriteTo(writer);
            EndTextMacroToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            context.TextMacros.Add(IdentifierName.Token.Text, this);
        }

        public override string ToString() => $"{PreprocessorDirectiveStartToken} {TextMacroToken} {IdentifierName}{ParameterList.OptionalPrefixed()} [...]";

        public override VJassSyntaxToken GetFirstToken() => PreprocessorDirectiveStartToken;

        public override VJassSyntaxToken GetLastToken() => EndTextMacroToken;

        protected internal override VJassTextMacroPreprocessorDirectiveTrivia ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTextMacroPreprocessorDirectiveTrivia(
                newToken,
                TextMacroToken,
                IdentifierName,
                ParameterList,
                Body,
                PreprocessorDirectiveEndToken,
                EndTextMacroToken);
        }

        protected internal override VJassTextMacroPreprocessorDirectiveTrivia ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassTextMacroPreprocessorDirectiveTrivia(
                PreprocessorDirectiveStartToken,
                TextMacroToken,
                IdentifierName,
                ParameterList,
                Body,
                PreprocessorDirectiveEndToken,
                newToken);
        }
    }
}