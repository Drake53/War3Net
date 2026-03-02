namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalDeclarationStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassGlobalDeclarationStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassGlobalDeclarationSyntax> globals)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            Globals = globals;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalDeclarationStaticIfClauseSyntax globalDeclarationStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(globalDeclarationStaticIfClause.StaticIfClauseDeclarator)
                && Globals.IsEquivalentTo(globalDeclarationStaticIfClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            StaticIfClauseDeclarator.ProcessTo(writer, context);
            Globals.ProcessTo(writer, context);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : Globals[^1].GetLastToken();

        protected internal override VJassGlobalDeclarationStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                Globals);
        }

        protected internal override VJassGlobalDeclarationStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassGlobalDeclarationStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassGlobalDeclarationStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                Globals);
        }
    }
}