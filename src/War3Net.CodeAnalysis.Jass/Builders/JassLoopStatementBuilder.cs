using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassLoopStatementBuilder : JassStatementListSyntaxBuilder
    {
        private readonly JassSyntaxToken _loopToken;

        public JassLoopStatementBuilder(JassSyntaxToken loopToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(loopToken, JassSyntaxKind.LoopKeyword);

            _loopToken = loopToken;
        }

        public JassLoopStatementSyntax ToLoopStatement(JassSyntaxToken endLoopToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(endLoopToken, JassSyntaxKind.EndLoopKeyword);

            return new JassLoopStatementSyntax(
                _loopToken,
                BuildStatementList(),
                endLoopToken.PrependLeadingTrivia(BuildTriviaList()));
        }
    }
}