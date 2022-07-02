// ------------------------------------------------------------------------------
// <copyright file="VJassDebugPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassDebugPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        internal VJassDebugPreprocessorDirectiveTrivia(
            VJassSyntaxToken debugToken)
        {
            DebugToken = debugToken;
        }

        public VJassSyntaxToken DebugToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassDebugPreprocessorDirectiveTrivia debugPreprocessorDirectiveTrivia
                && DebugToken.IsEquivalentTo(debugPreprocessorDirectiveTrivia.DebugToken);
        }

        public override void WriteTo(TextWriter writer)
        {
            DebugToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            DebugToken.ProcessTo(writer, context);
        }

        public override string ToString() => DebugToken.ToString();

        public override VJassSyntaxToken GetFirstToken() => DebugToken;

        public override VJassSyntaxToken GetLastToken() => DebugToken;

        protected internal override VJassDebugPreprocessorDirectiveTrivia ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassDebugPreprocessorDirectiveTrivia(newToken);
        }

        protected internal override VJassDebugPreprocessorDirectiveTrivia ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassDebugPreprocessorDirectiveTrivia(newToken);
        }
    }
}