// ------------------------------------------------------------------------------
// <copyright file="JassParameterSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassParameterSyntax : JassSyntaxNode
    {
        internal JassParameterSyntax(
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.Parameter;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassParameterSyntax parameter
                && Type.IsEquivalentTo(parameter.Type)
                && IdentifierName.IsEquivalentTo(parameter.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Type;
            yield return IdentifierName;
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
        }

        public override string ToString() => $"{Type} {IdentifierName}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override JassParameterSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassParameterSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName);
        }

        protected internal override JassParameterSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassParameterSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}