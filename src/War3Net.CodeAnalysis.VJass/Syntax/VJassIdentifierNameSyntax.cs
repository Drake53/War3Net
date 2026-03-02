using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIdentifierNameSyntax : VJassExpressionSyntax
    {
        internal VJassIdentifierNameSyntax(
            VJassSyntaxToken token)
        {
            Token = token;
        }

        public VJassSyntaxToken Token { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassIdentifierNameSyntax identifierName
                && Token.IsEquivalentTo(identifierName.Token);
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

        protected internal override VJassIdentifierNameSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassIdentifierNameSyntax(newToken);
        }

        protected internal override VJassIdentifierNameSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassIdentifierNameSyntax(newToken);
        }
    }
}