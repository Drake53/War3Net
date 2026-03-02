namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public MemberDeclarationSyntax Transpile(JassTypeDeclarationSyntax typeDeclaration)
        {
            var identifier = Transpile(typeDeclaration.IdentifierName.Token);

            var baseList = SyntaxFactory.BaseList(
                Transpile(SyntaxKind.ColonToken, typeDeclaration.ExtendsToken),
                SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                    SyntaxFactory.SimpleBaseType(Transpile(typeDeclaration.BaseType))));

            var indentationString = typeDeclaration.TypeToken.LeadingTrivia.GetIndentationString();

            return SyntaxFactory.ClassDeclaration(
                default,
                new SyntaxTokenList(TokenWithSpace(typeDeclaration.TypeToken.LeadingTrivia, SyntaxKind.PublicKeyword)),
                Transpile(SyntaxKind.ClassKeyword, typeDeclaration.TypeToken.TrailingTrivia),
                identifier,
                null,
                baseList,
                default,
                SyntaxFactory.Token(
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticWhitespace(indentationString)),
                    SyntaxKind.OpenBraceToken,
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticCarriageReturnLineFeed)),
                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                    SyntaxFactory.ConstructorDeclaration(
                        default,
                        new SyntaxTokenList(
                            SyntaxFactory.Token(
                                SyntaxTriviaList.Create(SyntaxFactory.ElasticWhitespace(indentationString)),
                                SyntaxKind.InternalKeyword,
                                SyntaxTriviaList.Create(SyntaxFactory.ElasticSpace))),
                        identifier.WithoutTrivia(),
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                            SyntaxFactory.SeparatedList<ParameterSyntax>(),
                            SyntaxFactory.Token(SyntaxKind.CloseParenToken).WithSpace()),
                        null,
                        SyntaxFactory.Block().WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed))),
                SyntaxFactory.Token(
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticWhitespace(indentationString)),
                    SyntaxKind.CloseBraceToken,
                    SyntaxTriviaList.Create(SyntaxFactory.ElasticCarriageReturnLineFeed)),
                default);
        }
    }
}