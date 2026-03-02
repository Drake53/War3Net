using System;
using System.Collections.Generic;
using War3Net.CodeAnalysis.VJass.Syntax;

namespace War3Net.CodeAnalysis.VJass
{
    public class VJassPreprocessorContext
    {
        public VJassPreprocessorContext()
        {
            KeepNoVJassPreprocessorDirectives = true;

            TextMacros = new Dictionary<string, VJassTextMacroPreprocessorDirectiveTrivia>(StringComparer.Ordinal);
        }

        public bool KeepNoVJassPreprocessorDirectives { get; }

        internal Dictionary<string, VJassTextMacroPreprocessorDirectiveTrivia> TextMacros { get; }
    }
}