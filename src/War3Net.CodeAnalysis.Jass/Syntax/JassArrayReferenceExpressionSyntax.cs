// ------------------------------------------------------------------------------
// <copyright file="JassArrayReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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