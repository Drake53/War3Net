// ------------------------------------------------------------------------------
// <copyright file="JassElementAccessExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElementAccessExpressionSyntax : JassExpressionSyntax
    {
        internal JassElementAccessExpressionSyntax(
            JassIdentifierNameSyntax identifierName,
            JassElementAccessClauseSyntax elementAccessClause)
        {
            IdentifierName = identifierName;
            ElementAccessClause = elementAccessClause;
        }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassElementAccessClauseSyntax ElementAccessClause { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ElementAccessExpression;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElementAccessExpressionSyntax elementAccessExpression
                && IdentifierName.IsEquivalentTo(elementAccessExpression.IdentifierName)
                && ElementAccessClause.IsEquivalentTo(elementAccessExpression.ElementAccessClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            IdentifierName.WriteTo(writer);
            ElementAccessClause.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;
            yield return ElementAccessClause;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return IdentifierName;
            yield return ElementAccessClause;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return ElementAccessClause;
            foreach (var descendant in ElementAccessClause.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ElementAccessClause.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ElementAccessClause;
            foreach (var descendant in ElementAccessClause.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{IdentifierName}{ElementAccessClause}";

        public override JassSyntaxToken GetFirstToken() => IdentifierName.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ElementAccessClause.GetLastToken();

        protected internal override JassElementAccessExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessExpressionSyntax(
                IdentifierName.ReplaceFirstToken(newToken),
                ElementAccessClause);
        }

        protected internal override JassElementAccessExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassElementAccessExpressionSyntax(
                IdentifierName,
                ElementAccessClause.ReplaceLastToken(newToken));
        }
    }
}