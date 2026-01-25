// ------------------------------------------------------------------------------
// <copyright file="JassLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLiteralExpressionSyntax : JassExpressionSyntax
    {
        internal JassLiteralExpressionSyntax(
            JassSyntaxToken token)
        {
            Token = token;
        }

        public JassSyntaxToken Token { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxFacts.GetLiteralExpressionKind(Token.SyntaxKind);

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassLiteralExpressionSyntax literalExpression
                && Token.IsEquivalentTo(literalExpression.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Token;
        }

        public override string ToString() => Token.ToString();

        public override JassSyntaxToken GetFirstToken() => Token;

        public override JassSyntaxToken GetLastToken() => Token;

        protected internal override JassLiteralExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassLiteralExpressionSyntax(newToken);
        }

        protected internal override JassLiteralExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassLiteralExpressionSyntax(newToken);
        }
    }
}