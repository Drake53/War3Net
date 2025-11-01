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
            JassEqualsValueClauseSyntax value)
        {
            SetToken = setToken;
            IdentifierName = identifierName;
            ElementAccessClause = elementAccessClause;
            Value = value;
        }

        public JassSyntaxToken SetToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassElementAccessClauseSyntax? ElementAccessClause { get; }

        public JassEqualsValueClauseSyntax Value { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.SetStatement;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassSetStatementSyntax setStatement
                && IdentifierName.IsEquivalentTo(setStatement.IdentifierName)
                && ElementAccessClause.NullableEquivalentTo(setStatement.ElementAccessClause)
                && Value.IsEquivalentTo(setStatement.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            SetToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ElementAccessClause?.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;

            if (ElementAccessClause is not null)
            {
                yield return ElementAccessClause;
            }

            yield return Value;
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

            yield return Value;
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

            yield return Value;
            foreach (var descendant in Value.GetDescendantNodes())
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

            foreach (var descendant in Value.GetDescendantTokens())
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

            yield return Value;
            foreach (var descendant in Value.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{SetToken} {IdentifierName}{ElementAccessClause.Optional()} {Value}";

        public override JassSyntaxToken GetFirstToken() => SetToken;

        public override JassSyntaxToken GetLastToken() => Value.GetLastToken();

        protected internal override JassSetStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                newToken,
                IdentifierName,
                ElementAccessClause,
                Value);
        }

        protected internal override JassSetStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                SetToken,
                IdentifierName,
                ElementAccessClause,
                Value.ReplaceLastToken(newToken));
        }
    }
}