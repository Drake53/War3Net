// ------------------------------------------------------------------------------
// <copyright file="JassGlobalConstantDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalConstantDeclarationSyntax : JassGlobalDeclarationSyntax
    {
        internal JassGlobalConstantDeclarationSyntax(
            JassSyntaxToken constantToken,
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax value)
        {
            ConstantToken = constantToken;
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public JassSyntaxToken ConstantToken { get; }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax Value { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.GlobalConstantDeclaration;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalConstantDeclarationSyntax globalConstantDeclaration
                && Type.IsEquivalentTo(globalConstantDeclaration.Type)
                && IdentifierName.IsEquivalentTo(globalConstantDeclaration.IdentifierName)
                && Value.IsEquivalentTo(globalConstantDeclaration.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            ConstantToken.WriteTo(writer);
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;
            yield return Value;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ConstantToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return ConstantToken;
            yield return Type;
            yield return IdentifierName;
            yield return Value;
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

            yield return Value;
            foreach (var descendant in Value.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return ConstantToken;

            foreach (var descendant in Type.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Value.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return ConstantToken;

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

            yield return Value;
            foreach (var descendant in Value.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{ConstantToken} {Type} {IdentifierName} {Value}";

        public override JassSyntaxToken GetFirstToken() => ConstantToken;

        public override JassSyntaxToken GetLastToken() => Value.GetLastToken();

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                newToken,
                Type,
                IdentifierName,
                Value);
        }

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                ConstantToken,
                Type,
                IdentifierName,
                Value.ReplaceLastToken(newToken));
        }
    }
}