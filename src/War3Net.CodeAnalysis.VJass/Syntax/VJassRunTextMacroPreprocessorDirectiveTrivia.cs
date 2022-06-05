// ------------------------------------------------------------------------------
// <copyright file="VJassRunTextMacroPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassRunTextMacroPreprocessorDirectiveTrivia : ISyntaxTrivia
    {
        public VJassRunTextMacroPreprocessorDirectiveTrivia(
            string name,
            ImmutableArray<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; }

        public ImmutableArray<string> Arguments { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassRunTextMacroPreprocessorDirectiveTrivia runTextMacroPreprocessorDirectiveTrivia
                && string.Equals(Name, runTextMacroPreprocessorDirectiveTrivia.Name, StringComparison.Ordinal)
                && Arguments.SequenceEqual(runTextMacroPreprocessorDirectiveTrivia.Arguments, StringComparer.Ordinal);
        }

        public override string ToString() => $"{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.RunTextMacro} {Name}({string.Join($"{VJassSymbol.Comma} ", Arguments.Select(arg => $"{VJassSymbol.QuotationMark}{arg}{VJassSymbol.QuotationMark}"))})";
    }
}