namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassGlobalDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassGlobalDeclarationSyntax> globals)
        {
            ElseToken = elseToken;
            Globals = globals;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalDeclarationElseClauseSyntax globalDeclarationElseClause
                && Globals.IsEquivalentTo(globalDeclarationElseClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseToken.ProcessTo(writer, context);
            Globals.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? ElseToken : Globals[^1].GetLastToken();

        protected internal override VJassGlobalDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }

        protected internal override VJassGlobalDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassGlobalDeclarationElseClauseSyntax(
                    ElseToken,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }
    }
}