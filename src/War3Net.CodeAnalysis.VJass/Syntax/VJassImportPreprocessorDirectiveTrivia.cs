// ------------------------------------------------------------------------------
// <copyright file="VJassImportPreprocessorDirectiveTrivia.cs" company="Drake53">
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
    public class VJassImportPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        internal VJassImportPreprocessorDirectiveTrivia(
            VJassSyntaxToken preprocessorDirectiveToken,
            VJassSyntaxToken importToken,
            VJassImportScriptTypeSyntax? importScriptType,
            VJassExpressionSyntax scriptFilePath)
        {
            PreprocessorDirectiveToken = preprocessorDirectiveToken;
            ImportToken = importToken;
            ImportScriptType = importScriptType;
            ScriptFilePath = scriptFilePath;
        }

        public VJassSyntaxToken PreprocessorDirectiveToken { get; }

        public VJassSyntaxToken ImportToken { get; }

        public VJassImportScriptTypeSyntax? ImportScriptType { get; }

        public VJassExpressionSyntax ScriptFilePath { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassImportPreprocessorDirectiveTrivia importPreprocessorDirectiveTrivia
                && ImportScriptType.NullableEquivalentTo(importPreprocessorDirectiveTrivia.ImportScriptType)
                && ScriptFilePath.IsEquivalentTo(importPreprocessorDirectiveTrivia.ScriptFilePath);
        }

        public override void WriteTo(TextWriter writer)
        {
            PreprocessorDirectiveToken.WriteTo(writer);
            ImportToken.WriteTo(writer);
            ImportScriptType?.WriteTo(writer);
            ScriptFilePath.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => $"{PreprocessorDirectiveToken} {ImportToken} {ImportScriptType.OptionalSuffixed()}{ScriptFilePath}";

        public override VJassSyntaxToken GetFirstToken() => PreprocessorDirectiveToken;

        public override VJassSyntaxToken GetLastToken() => ScriptFilePath.GetLastToken();

        protected internal override VJassImportPreprocessorDirectiveTrivia ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassImportPreprocessorDirectiveTrivia(
                newToken,
                ImportToken,
                ImportScriptType,
                ScriptFilePath);
        }

        protected internal override VJassImportPreprocessorDirectiveTrivia ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassImportPreprocessorDirectiveTrivia(
                PreprocessorDirectiveToken,
                ImportToken,
                ImportScriptType,
                ScriptFilePath.ReplaceLastToken(newToken));
        }
    }
}