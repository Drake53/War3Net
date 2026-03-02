namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStaticIfTopLevelDeclarationSyntax : VJassTopLevelDeclarationSyntax
    {
        internal VJassStaticIfTopLevelDeclarationSyntax(
            VJassTopLevelDeclarationStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassTopLevelDeclarationElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassTopLevelDeclarationStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassTopLevelDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassTopLevelDeclarationElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfTopLevelDeclarationSyntax staticIfTopLevelDeclaration
                && StaticIfClause.IsEquivalentTo(staticIfTopLevelDeclaration.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfTopLevelDeclaration.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(staticIfTopLevelDeclaration.ElseClause);
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

        protected internal override VJassStaticIfTopLevelDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfTopLevelDeclarationSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfTopLevelDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfTopLevelDeclarationSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}