// ------------------------------------------------------------------------------
// <copyright file="JassLoopStatementBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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
                endLoopToken.PrependTrivia(BuildTriviaList()));
        }
    }
}