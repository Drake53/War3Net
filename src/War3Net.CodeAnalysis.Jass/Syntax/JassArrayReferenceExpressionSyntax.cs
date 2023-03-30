// ------------------------------------------------------------------------------
// <copyright file="JassArrayReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArrayReferenceExpressionSyntax : JassExpressionSyntax
    {
        internal JassArrayReferenceExpressionSyntax(
            JassIdentifierNameSyntax identifierName,
            JassElementAccessClauseSyntax elementAccessClause)
        {
            IdentifierName = identifierName;
            ElementAccessClause = elementAccessClause;
        }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassElementAccessClauseSyntax ElementAccessClause { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ArrayReferenceExpression;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassArrayReferenceExpressionSyntax arrayReferenceExpression
                && IdentifierName.IsEquivalentTo(arrayReferenceExpression.IdentifierName)
                && ElementAccessClause.IsEquivalentTo(arrayReferenceExpression.ElementAccessClause);
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

        protected internal override JassArrayReferenceExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassArrayReferenceExpressionSyntax(
                IdentifierName.ReplaceFirstToken(newToken),
                ElementAccessClause);
        }

        protected internal override JassArrayReferenceExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassArrayReferenceExpressionSyntax(
                IdentifierName,
                ElementAccessClause.ReplaceLastToken(newToken));
        }
    }
}