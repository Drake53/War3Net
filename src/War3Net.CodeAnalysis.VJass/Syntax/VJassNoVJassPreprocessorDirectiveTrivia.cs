// ------------------------------------------------------------------------------
// <copyright file="VJassNoVJassPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassNoVJassPreprocessorDirectiveTrivia : ISyntaxTrivia
    {
        public VJassNoVJassPreprocessorDirectiveTrivia(string body)
        {
            Body = body;
        }

        public string Body { get; }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassNoVJassPreprocessorDirectiveTrivia noVJassPreprocessorDirectiveTrivia
                && string.Equals(Body, noVJassPreprocessorDirectiveTrivia.Body, StringComparison.Ordinal);
        }

        public override string ToString() => $@"{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.NoVJass}
{Body}
{VJassSymbol.Slash}{VJassSymbol.Slash}{VJassSymbol.ExclamationMark} {VJassKeyword.EndNoVJass}";
    }
}