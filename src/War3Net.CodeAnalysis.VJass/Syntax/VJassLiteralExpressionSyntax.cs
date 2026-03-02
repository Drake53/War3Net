using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLiteralExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassLiteralExpressionSyntax(
            VJassSyntaxToken token)
        {
            Token = token;
        }

        public VJassSyntaxToken Token { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLiteralExpressionSyntax literalExpression
                && Token.IsEquivalentTo(literalExpression.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Token.ProcessTo(writer, context);
        }

        public override string ToString() => Token.ToString();

        public override VJassSyntaxToken GetFirstToken() => Token;

        public override VJassSyntaxToken GetLastToken() => Token;

        protected internal override VJassLiteralExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassLiteralExpressionSyntax(newToken);
        }

        protected internal override VJassLiteralExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassLiteralExpressionSyntax(newToken);
        }
    }
}