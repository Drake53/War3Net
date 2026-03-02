namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStaticIfGlobalDeclarationSyntax : VJassGlobalDeclarationSyntax
    {
        internal VJassStaticIfGlobalDeclarationSyntax(
            VJassGlobalDeclarationStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassGlobalDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassGlobalDeclarationElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassGlobalDeclarationStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassGlobalDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassGlobalDeclarationElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfGlobalDeclarationSyntax staticIfGlobalDeclaration
                && StaticIfClause.IsEquivalentTo(staticIfGlobalDeclaration.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfGlobalDeclaration.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(staticIfGlobalDeclaration.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            StaticIfClause.ProcessTo(writer, context);
            ElseIfClauses.ProcessTo(writer, context);
            ElseClause?.ProcessTo(writer, context);
            EndIfToken.ProcessTo(writer, context);
        }

        public override string ToString() => StaticIfClause.ToString();

        public override VJassSyntaxToken GetFirstToken() => StaticIfClause.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override VJassStaticIfGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfGlobalDeclarationSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfGlobalDeclarationSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}