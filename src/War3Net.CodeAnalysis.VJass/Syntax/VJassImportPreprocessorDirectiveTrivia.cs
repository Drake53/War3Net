// ------------------------------------------------------------------------------
// <copyright file="VJassImportPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassImportPreprocessorDirectiveTrivia : ISyntaxTrivia
    {
        public VJassImportPreprocessorDirectiveTrivia(
            VJassImportScriptTypeSyntax? importScriptType,
            string scriptFilePath)
        {
            ImportScriptType = importScriptType;
            ScriptFilePath = scriptFilePath;
        }

        public VJassImportScriptTypeSyntax? ImportScriptType { get; }

        public string ScriptFilePath { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassImportPreprocessorDirectiveTrivia importPreprocessorDirectiveTrivia
                && ImportScriptType.Equals(importPreprocessorDirectiveTrivia.ImportScriptType)
                && string.Equals(Path.GetFullPath(ScriptFilePath), Path.GetFullPath(importPreprocessorDirectiveTrivia.ScriptFilePath), StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString() => $"{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.Import} {ImportScriptType} \"{ScriptFilePath}\"";
    }
}