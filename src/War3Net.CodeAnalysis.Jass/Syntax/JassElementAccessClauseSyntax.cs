// ------------------------------------------------------------------------------
// <copyright file="JassElementAccessClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElementAccessClauseSyntax : JassExpressionSyntax
    {
        internal JassElementAccessClauseSyntax(
            JassSyntaxToken openBracketToken,
            JassExpressionSyntax expression,
            JassSyntaxToken closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            Expression = expression;
            CloseBracketToken = closeBracketToken;
        }

        public JassSyntaxToken OpenBracketToken { get; }

        public JassExpressionSyntax Expression { get; }

        public JassSyntaxToken CloseBracketToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElementAccessClauseSyntax elementAccessClause
                && Expression.IsEquivalentTo(elementAccessClause.Expression);
        }

        public override void WriteTo(TextWriter writer)
        {
            OpenBracketToken.WriteTo(writer);
            Expression.WriteTo(writer);
            CloseBracketToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Expression;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return OpenBracketToken;
            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return OpenBracketToken;
            yield return Expression;
            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return OpenBracketToken;

            foreach (var descendant in Expression.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return CloseBracketToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return OpenBracketToken;

            yield return Expression;
            foreach (var descendant in Expression.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return CloseBracketToken;
        }

        public override string ToString() => $"{OpenBracketToken}{Expression}{CloseBracketToken}";

        public override JassSyntaxToken GetFirstToken() => OpenBracketToken;

        public override JassSyntaxToken GetLastToken() => CloseBracketToken;

        protected internal override JassElementAccessClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessClauseSyntax(
                newToken,
                Expression,
                CloseBracketToken);
        }

        protected internal override JassElementAccessClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessClauseSyntax(
                OpenBracketToken,
                Expression,
                newToken);
        }
    }
}