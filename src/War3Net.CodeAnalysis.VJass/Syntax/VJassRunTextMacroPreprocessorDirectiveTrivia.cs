// ------------------------------------------------------------------------------
// <copyright file="VJassRunTextMacroPreprocessorDirectiveTrivia.cs" company="Drake53">
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
    public class VJassRunTextMacroPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        internal VJassRunTextMacroPreprocessorDirectiveTrivia(
            VJassSyntaxToken preprocessorDirectiveToken,
            VJassSyntaxToken runTextMacroToken,
            VJassSyntaxToken? optionalToken,
            VJassIdentifierNameSyntax identifierName,
            VJassArgumentListSyntax argumentList)
        {
            PreprocessorDirectiveToken = preprocessorDirectiveToken;
            RunTextMacroToken = runTextMacroToken;
            OptionalToken = optionalToken;
            IdentifierName = identifierName;
            ArgumentList = argumentList;
        }

        public VJassSyntaxToken PreprocessorDirectiveToken { get; }

        public VJassSyntaxToken RunTextMacroToken { get; }

        public VJassSyntaxToken? OptionalToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassRunTextMacroPreprocessorDirectiveTrivia runTextMacroPreprocessorDirectiveTrivia
                && OptionalToken.NullableEquals(runTextMacroPreprocessorDirectiveTrivia.OptionalToken)
                && IdentifierName.IsEquivalentTo(runTextMacroPreprocessorDirectiveTrivia.IdentifierName)
                && ArgumentList.IsEquivalentTo(runTextMacroPreprocessorDirectiveTrivia.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            PreprocessorDirectiveToken.WriteTo(writer);
            RunTextMacroToken.WriteTo(writer);
            OptionalToken?.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ArgumentList.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => $"{PreprocessorDirectiveToken} {RunTextMacroToken} {OptionalToken.OptionalSuffixed()}{IdentifierName}{ArgumentList}";

        public override VJassSyntaxToken GetFirstToken() => PreprocessorDirectiveToken;

        public override VJassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override VJassRunTextMacroPreprocessorDirectiveTrivia ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassRunTextMacroPreprocessorDirectiveTrivia(
                newToken,
                RunTextMacroToken,
                OptionalToken,
                IdentifierName,
                ArgumentList);
        }

        protected internal override VJassRunTextMacroPreprocessorDirectiveTrivia ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassRunTextMacroPreprocessorDirectiveTrivia(
                PreprocessorDirectiveToken,
                RunTextMacroToken,
                OptionalToken,
                IdentifierName,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}