namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(
            JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            return Transpile(
                globalConstantDeclaration.GetLeadingTrivia(),
                globalConstantDeclaration,
                globalConstantDeclaration.GetTrailingTrivia());
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            return Transpile(
                leadingTrivia,
                globalConstantDeclaration,
                globalConstantDeclaration.GetTrailingTrivia());
        }

        public MemberDeclarationSyntax Transpile(
            JassGlobalConstantDeclarationSyntax globalConstantDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            return Transpile(
                globalConstantDeclaration.GetLeadingTrivia(),
                globalConstantDeclaration,
                trailingTrivia);
        }

        public MemberDeclarationSyntax Transpile(
            JassSyntaxTriviaList leadingTrivia,
            JassGlobalConstantDeclarationSyntax globalConstantDeclaration,
            JassSyntaxTriviaList trailingTrivia)
        {
            var variableDeclaration = SyntaxFactory.VariableDeclaration(
                TranspileAligned(globalConstantDeclaration.Type, isArray: false),
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.VariableDeclarator(
                        Transpile(globalConstantDeclaration.IdentifierName.Token),
                        null,
                        Transpile(globalConstantDeclaration.Value))));

            var declaration = SyntaxFactory.FieldDeclaration(
                default,
                new SyntaxTokenList(
                    TokenWithSpace(leadingTrivia, SyntaxKind.PublicKeyword),
                    Transpile(SyntaxKind.ConstKeyword, globalConstantDeclaration.ConstantToken)),
                variableDeclaration,
                Transpile(SyntaxKind.SemicolonToken, globalConstantDeclaration.GetTrailingTrivia()));

            if (ApplyCSharpLuaTemplateAttribute)
            {
                var jassToLuaTranspiler = JassToLuaTranspiler ?? new JassToLuaTranspiler();

                declaration = declaration.WithCSharpLuaTemplateAttribute(
                    jassToLuaTranspiler.Transpile(globalConstantDeclaration.IdentifierName.Token));
            }

            return declaration;
        }
    }
}