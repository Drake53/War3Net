using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassDebugPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        public static readonly VJassDebugPreprocessorDirectiveTrivia Value = new(new VJassSyntaxToken(VJassSyntaxKind.DebugKeyword, VJassKeyword.Debug, VJassSyntaxTriviaList.Empty));

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