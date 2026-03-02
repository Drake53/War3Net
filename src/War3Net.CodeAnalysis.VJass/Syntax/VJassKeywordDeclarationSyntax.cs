namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassKeywordDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassKeywordDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassSyntaxToken keywordToken,
            VJassIdentifierNameSyntax keyword)
        {
            Modifiers = modifiers;
            KeywordToken = keywordToken;
            Keyword = keyword;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassSyntaxToken KeywordToken { get; }

        public VJassIdentifierNameSyntax Keyword { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassKeywordDeclarationSyntax keywordDeclaration
                && Modifiers.IsEquivalentTo(keywordDeclaration.Modifiers)
                && Keyword.IsEquivalentTo(keywordDeclaration.Keyword);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            KeywordToken.WriteTo(writer);
            Keyword.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Modifiers.ProcessTo(writer, context);
            KeywordToken.ProcessTo(writer, context);
            Keyword.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Modifiers.Join()}{KeywordToken} {Keyword}";

        public override VJassSyntaxToken GetFirstToken() => Modifiers.IsEmpty ? KeywordToken : Modifiers[0].GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Keyword.GetLastToken();

        protected internal override VJassKeywordDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassKeywordDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    KeywordToken,
                    Keyword);
            }

            return new VJassKeywordDeclarationSyntax(
                Modifiers,
                newToken,
                Keyword);
        }

        protected internal override VJassKeywordDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassKeywordDeclarationSyntax(
                Modifiers,
                KeywordToken,
                Keyword.ReplaceLastToken(newToken));
        }
    }
}