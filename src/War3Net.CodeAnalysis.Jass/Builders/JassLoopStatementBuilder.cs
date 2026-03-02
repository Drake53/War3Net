namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassLoopStatementBuilder : JassStatementListSyntaxBuilder
    {
        private readonly JassSyntaxToken _loopToken;

        public JassLoopStatementBuilder(JassSyntaxToken loopToken)
        {
            ThrowHelper.ThrowIfInvalidToken(loopToken, JassSyntaxKind.LoopKeyword);

            _loopToken = loopToken;
        }

        public JassLoopStatementSyntax ToLoopStatement(JassSyntaxToken endLoopToken)
        {
            ThrowHelper.ThrowIfInvalidToken(endLoopToken, JassSyntaxKind.EndLoopKeyword);

            return new JassLoopStatementSyntax(
                _loopToken,
                BuildStatementList(),
                endLoopToken.PrependLeadingTrivia(BuildTriviaList()));
        }
    }
}