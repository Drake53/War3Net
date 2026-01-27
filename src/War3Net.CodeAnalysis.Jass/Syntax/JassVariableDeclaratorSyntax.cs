// ------------------------------------------------------------------------------
// <copyright file="JassVariableDeclaratorSyntax.cs" company="Drake53">
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
    public class JassVariableDeclaratorSyntax : JassVariableOrArrayDeclaratorSyntax
    {
        internal JassVariableDeclaratorSyntax(
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax? equalsValueClause)
        {
            Type = type;
            IdentifierName = identifierName;
            EqualsValueClause = equalsValueClause;
        }

        public override JassTypeSyntax Type { get; }

        public override JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax? EqualsValueClause { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.VariableDeclarator;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassVariableDeclaratorSyntax variableDeclarator
                && Type.IsEquivalentTo(variableDeclarator.Type)
                && IdentifierName.IsEquivalentTo(variableDeclarator.IdentifierName)
                && EqualsValueClause.NullableEquivalentTo(variableDeclarator.EqualsValueClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            EqualsValueClause?.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;

            if (EqualsValueClause is not null)
            {
                yield return EqualsValueClause;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Type;
            yield return IdentifierName;

            if (EqualsValueClause is not null)
            {
                yield return EqualsValueClause;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Type;
            foreach (var descendant in Type.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            if (EqualsValueClause is not null)
            {
                yield return EqualsValueClause;
                foreach (var descendant in EqualsValueClause.GetDescendantNodes())
                {
                    yield return descendant;
                }
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in Type.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            if (EqualsValueClause is not null)
            {
                foreach (var descendant in EqualsValueClause.GetDescendantTokens())
                {
                    yield return descendant;
                }
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Type;
            foreach (var descendant in Type.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            if (EqualsValueClause is not null)
            {
                yield return EqualsValueClause;
                foreach (var descendant in EqualsValueClause.GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
            }
        }

        public override string ToString() => $"{Type} {IdentifierName}{EqualsValueClause.OptionalPrefixed()}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ((JassSyntaxNode?)EqualsValueClause ?? IdentifierName).GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitVariableDeclarator(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitVariableDeclarator(this);

        public JassVariableDeclaratorSyntax Update(
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax? equalsValueClause)
        {
            if (ReferenceEquals(Type, type) &&
                ReferenceEquals(IdentifierName, identifierName) &&
                ReferenceEquals(EqualsValueClause, equalsValueClause))
            {
                return this;
            }

            return new JassVariableDeclaratorSyntax(type, identifierName, equalsValueClause);
        }

        public JassVariableDeclaratorSyntax WithType(JassTypeSyntax type) => Update(type, IdentifierName, EqualsValueClause);

        public JassVariableDeclaratorSyntax WithIdentifierName(JassIdentifierNameSyntax identifierName) => Update(Type, identifierName, EqualsValueClause);

        public JassVariableDeclaratorSyntax WithEqualsValueClause(JassEqualsValueClauseSyntax? equalsValueClause) => Update(Type, IdentifierName, equalsValueClause);

        protected internal override JassVariableDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassVariableDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName,
                EqualsValueClause);
        }

        protected internal override JassVariableDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (EqualsValueClause is not null)
            {
                return new JassVariableDeclaratorSyntax(
                    Type,
                    IdentifierName,
                    EqualsValueClause.ReplaceLastToken(newToken));
            }

            return new JassVariableDeclaratorSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}