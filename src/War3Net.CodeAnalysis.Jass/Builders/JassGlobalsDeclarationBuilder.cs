namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassGlobalsDeclarationBuilder : JassSyntaxBuilder
    {
        private readonly ImmutableArray<JassGlobalDeclarationSyntax>.Builder _globalDeclarationsBuilder;
        private readonly JassSyntaxToken _globalsToken;

        public JassGlobalsDeclarationBuilder(JassSyntaxToken globalsToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(globalsToken, JassSyntaxKind.GlobalsKeyword);

            _globalDeclarationsBuilder = ImmutableArray.CreateBuilder<JassGlobalDeclarationSyntax>();
            _globalsToken = globalsToken;
        }

        public JassGlobalsDeclarationSyntax ToGlobalsDeclaration(JassSyntaxToken endGlobalsToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(endGlobalsToken, JassSyntaxKind.EndGlobalsKeyword);

            return new JassGlobalsDeclarationSyntax(
                _globalsToken,
                _globalDeclarationsBuilder.ToImmutable(),
                endGlobalsToken.PrependLeadingTrivia(BuildTriviaList()));
        }

        public void AddGlobalDeclaration(JassGlobalDeclarationSyntax globalDeclaration)
        {
            _globalDeclarationsBuilder.Add(globalDeclaration.PrependLeadingTrivia(BuildTriviaList()));
        }
    }
}