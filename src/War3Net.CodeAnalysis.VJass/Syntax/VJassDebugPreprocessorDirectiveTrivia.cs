// ------------------------------------------------------------------------------
// <copyright file="VJassDebugPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassDebugPreprocessorDirectiveTrivia : ISyntaxTrivia
    {
        public static readonly VJassDebugPreprocessorDirectiveTrivia Value = new VJassDebugPreprocessorDirectiveTrivia();

        private VJassDebugPreprocessorDirectiveTrivia()
        {
        }

        public bool Equals(ISyntaxTrivia? other)
        {
            return other is VJassDebugPreprocessorDirectiveTrivia;
        }

        public override string ToString() => VJassKeyword.Debug;
    }
}