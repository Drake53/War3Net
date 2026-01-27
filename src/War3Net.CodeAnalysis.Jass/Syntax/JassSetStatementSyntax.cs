// ------------------------------------------------------------------------------
// <copyright file="JassSetStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSetStatementSyntax : JassStatementSyntax
    {
        internal JassSetStatementSyntax(
            JassSyntaxToken setToken,
            JassIdentifierNameSyntax identifierName,
            JassElementAccessClauseSyntax? elementAccessClause,
            JassEqualsValueClauseSyntax equalsValueClause)
        {
            SetToken = setToken;
            IdentifierName = identifierName;
            ElementAccessClause = elementAccessClause;
            EqualsValueClause = equalsValueClause;
        }

        public JassSyntaxToken SetToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassElementAccessClauseSyntax? ElementAccessClause { get; }

        public JassEqualsValueClauseSyntax EqualsValueClause { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.SetStatement;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassSetStatementSyntax setStatement
                && IdentifierName.IsEquivalentTo(setStatement.IdentifierName)
                && ElementAccessClause.NullableEquivalentTo(setStatement.ElementAccessClause)
                && EqualsValueClause.IsEquivalentTo(setStatement.EqualsValueClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            SetToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ElementAccessClause?.WriteTo(writer);
            EqualsValueClause.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;

            if (ElementAccessClause is not null)
            {
                yield return ElementAccessClause;
            }

            yield return EqualsValueClause;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return SetToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return SetToken;
            yield return IdentifierName;

            if (ElementAccessClause is not null)
            {
                yield return ElementAccessClause;
            }

            yield return EqualsValueClause;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            if (ElementAccessClause is not null)
            {
                yield return ElementAccessClause;
                foreach (var descendant in ElementAccessClause.GetDescendantNodes())
                {
                    yield return descendant;
                }
            }

            yield return EqualsValueClause;
            foreach (var descendant in EqualsValueClause.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return SetToken;

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            if (ElementAccessClause is not null)
            {
                foreach (var descendant in ElementAccessClause.GetDescendantTokens())
                {
                    yield return descendant;
                }
            }

            foreach (var descendant in EqualsValueClause.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return SetToken;

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            if (ElementAccessClause is not null)
            {
                yield return ElementAccessClause;
                foreach (var descendant in ElementAccessClause.GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
            }

            yield return EqualsValueClause;
            foreach (var descendant in EqualsValueClause.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{SetToken} {IdentifierName}{ElementAccessClause.Optional()} {EqualsValueClause}";

        public override JassSyntaxToken GetFirstToken() => SetToken;

        public override JassSyntaxToken GetLastToken() => EqualsValueClause.GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitSetStatement(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitSetStatement(this);

        public JassSetStatementSyntax Update(
            JassSyntaxToken setToken,
            JassIdentifierNameSyntax identifierName,
            JassElementAccessClauseSyntax? elementAccessClause,
            JassEqualsValueClauseSyntax equalsValueClause)
        {
            if (ReferenceEquals(SetToken, setToken) &&
                ReferenceEquals(IdentifierName, identifierName) &&
                ReferenceEquals(ElementAccessClause, elementAccessClause) &&
                ReferenceEquals(EqualsValueClause, equalsValueClause))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(setToken, JassSyntaxKind.SetKeyword);

            return new JassSetStatementSyntax(setToken, identifierName, elementAccessClause, equalsValueClause);
        }

        public JassSetStatementSyntax WithSetToken(JassSyntaxToken setToken) => Update(setToken, IdentifierName, ElementAccessClause, EqualsValueClause);

        public JassSetStatementSyntax WithIdentifierName(JassIdentifierNameSyntax identifierName) => Update(SetToken, identifierName, ElementAccessClause, EqualsValueClause);

        public JassSetStatementSyntax WithElementAccessClause(JassElementAccessClauseSyntax? elementAccessClause) => Update(SetToken, IdentifierName, elementAccessClause, EqualsValueClause);

        public JassSetStatementSyntax WithEqualsValueClause(JassEqualsValueClauseSyntax equalsValueClause) => Update(SetToken, IdentifierName, ElementAccessClause, equalsValueClause);

        protected internal override JassSetStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                newToken,
                IdentifierName,
                ElementAccessClause,
                EqualsValueClause);
        }

        protected internal override JassSetStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                SetToken,
                IdentifierName,
                ElementAccessClause,
                EqualsValueClause.ReplaceLastToken(newToken));
        }
    }
}