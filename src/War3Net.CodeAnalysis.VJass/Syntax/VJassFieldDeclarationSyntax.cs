namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFieldDeclarationSyntax : VJassMemberDeclarationSyntax
    {
        internal VJassFieldDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassVariableOrArrayDeclaratorSyntax declarator)
        {
            Modifiers = modifiers;
            Declarator = declarator;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassFieldDeclarationSyntax fieldDeclaration
                && Modifiers.IsEquivalentTo(fieldDeclaration.Modifiers)
                && Declarator.IsEquivalentTo(fieldDeclaration.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            Declarator.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Modifiers.ProcessTo(writer, context);
            Declarator.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Modifiers.Join()}{Declarator}";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)Declarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override VJassFieldDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassFieldDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    Declarator);
            }

            return new VJassFieldDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override VJassFieldDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFieldDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceLastToken(newToken));
        }
    }
}