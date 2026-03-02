namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStructDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassStructDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassStructDeclaratorSyntax declarator,
            ImmutableArray<VJassMemberDeclarationSyntax> memberDeclarations,
            VJassSyntaxToken endStructToken)
        {
            Modifiers = modifiers;
            Declarator = declarator;
            MemberDeclarations = memberDeclarations;
            EndStructToken = endStructToken;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassStructDeclaratorSyntax Declarator { get; }

        public ImmutableArray<VJassMemberDeclarationSyntax> MemberDeclarations { get; }

        public VJassSyntaxToken EndStructToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStructDeclarationSyntax structDeclaration
                && Modifiers.IsEquivalentTo(structDeclaration.Modifiers)
                && Declarator.IsEquivalentTo(structDeclaration.Declarator)
                && MemberDeclarations.IsEquivalentTo(structDeclaration.MemberDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            Declarator.WriteTo(writer);
            MemberDeclarations.WriteTo(writer);
            EndStructToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Modifiers.ProcessTo(writer, context);
            Declarator.ProcessTo(writer, context);
            MemberDeclarations.ProcessTo(writer, context);
            EndStructToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Modifiers.Join()}{Declarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)Declarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndStructToken;

        protected internal override VJassStructDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassStructDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    Declarator,
                    MemberDeclarations,
                    EndStructToken);
            }

            return new VJassStructDeclarationSyntax(
                Modifiers,
                Declarator.ReplaceFirstToken(newToken),
                MemberDeclarations,
                EndStructToken);
        }

        protected internal override VJassStructDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStructDeclarationSyntax(
                Modifiers,
                Declarator,
                MemberDeclarations,
                newToken);
        }
    }
}