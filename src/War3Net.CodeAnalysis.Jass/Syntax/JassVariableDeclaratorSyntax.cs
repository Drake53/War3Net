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
            JassEqualsValueClauseSyntax? value)
        {
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax? Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassVariableDeclaratorSyntax variableDeclarator
                && Type.IsEquivalentTo(variableDeclarator.Type)
                && IdentifierName.IsEquivalentTo(variableDeclarator.IdentifierName)
                && Value.NullableEquivalentTo(variableDeclarator.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Value?.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;

            if (Value is not null)
            {
                yield return Value;
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

            if (Value is not null)
            {
                yield return Value;
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

            if (Value is not null)
            {
                yield return Value;
                foreach (var descendant in Value.GetDescendantNodes())
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

            if (Value is not null)
            {
                foreach (var descendant in Value.GetDescendantTokens())
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

            if (Value is not null)
            {
                yield return Value;
                foreach (var descendant in Value.GetDescendantNodesAndTokens())
                {
                    yield return descendant;
                }
            }
        }

        public override string ToString() => $"{Type} {IdentifierName}{Value.OptionalPrefixed()}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ((JassSyntaxNode?)Value ?? IdentifierName).GetLastToken();

        protected internal override JassVariableDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassVariableDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName,
                Value);
        }

        protected internal override JassVariableDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (Value is not null)
            {
                return new JassVariableDeclaratorSyntax(
                    Type,
                    IdentifierName,
                    Value.ReplaceLastToken(newToken));
            }

            return new JassVariableDeclaratorSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}