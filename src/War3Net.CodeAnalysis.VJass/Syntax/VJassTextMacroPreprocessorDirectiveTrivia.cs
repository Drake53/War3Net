// ------------------------------------------------------------------------------
// <copyright file="VJassTextMacroPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTextMacroPreprocessorDirectiveTrivia : ISyntaxTrivia
    {
        public VJassTextMacroPreprocessorDirectiveTrivia(
            string name,
            ImmutableArray<string> parameters,
            string body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }

        public string Name { get; }

        public ImmutableArray<string> Parameters { get; }

        public string Body { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassTextMacroPreprocessorDirectiveTrivia textMacroPreprocessorDirectiveTrivia
                && string.Equals(Name, textMacroPreprocessorDirectiveTrivia.Name, StringComparison.Ordinal)
                && Parameters.SequenceEqual(textMacroPreprocessorDirectiveTrivia.Parameters, StringComparer.Ordinal)
                && string.Equals(Body, textMacroPreprocessorDirectiveTrivia.Body, StringComparison.Ordinal);
        }

        public override string ToString() => Parameters.IsEmpty
            ? $@"{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.TextMacro} {Name}
{Body}
{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.EndTextMacro}"
            : $@"{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.TextMacro} {Name} {VJassKeyword.Takes} {string.Join($"{VJassSymbol.Comma} ", Parameters)}
{Body}
{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.EndTextMacro}";
    }
}