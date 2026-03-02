namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopedDeclarationElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedDeclarationElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassScopedDeclarationSyntax> declarations)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Declarations = declarations;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassScopedDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedDeclarationElseIfClauseSyntax scopedDeclarationElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(scopedDeclarationElseIfClause.ElseIfClauseDeclarator)
                && Declarations.IsEquivalentTo(scopedDeclarationElseIfClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseIfClauseDeclarator.ProcessTo(writer, context);
            Declarations.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Declarations[^1].GetLastToken();

        protected internal override VJassScopedDeclarationElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Declarations);
        }

        protected internal override VJassScopedDeclarationElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassScopedDeclarationElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Declarations);
        }
    }
}