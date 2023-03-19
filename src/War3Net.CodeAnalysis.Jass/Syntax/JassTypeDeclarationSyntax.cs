// ------------------------------------------------------------------------------
// <copyright file="JassTypeDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassTypeDeclarationSyntax : JassTopLevelDeclarationSyntax
    {
        internal JassTypeDeclarationSyntax(
            JassSyntaxToken typeToken,
            JassIdentifierNameSyntax identifierName,
            JassSyntaxToken extendsToken,
            JassTypeSyntax baseType)
        {
            TypeToken = typeToken;
            IdentifierName = identifierName;
            ExtendsToken = extendsToken;
            BaseType = baseType;
        }

        public JassSyntaxToken TypeToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassSyntaxToken ExtendsToken { get; }

        public JassTypeSyntax BaseType { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.IsEquivalentTo(typeDeclaration.IdentifierName)
                && BaseType.IsEquivalentTo(typeDeclaration.BaseType);
        }

        public override void WriteTo(TextWriter writer)
        {
            TypeToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ExtendsToken.WriteTo(writer);
            BaseType.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;
            yield return BaseType;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return TypeToken;
            yield return ExtendsToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return TypeToken;
            yield return IdentifierName;
            yield return ExtendsToken;
            yield return BaseType;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in BaseType.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return BaseType;
            foreach (var descendant in BaseType.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return TypeToken;

            foreach (var descendant in BaseType.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return ExtendsToken;

            foreach (var descendant in BaseType.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return TypeToken;

            yield return IdentifierName;
            foreach (var descendant in BaseType.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ExtendsToken;

            yield return BaseType;
            foreach (var descendant in BaseType.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{TypeToken} {IdentifierName} {ExtendsToken} {BaseType}";

        public override JassSyntaxToken GetFirstToken() => TypeToken;

        public override JassSyntaxToken GetLastToken() => BaseType.GetLastToken();

        protected internal override JassTypeDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassTypeDeclarationSyntax(
                newToken,
                IdentifierName,
                ExtendsToken,
                BaseType);
        }

        protected internal override JassTypeDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassTypeDeclarationSyntax(
                TypeToken,
                IdentifierName,
                ExtendsToken,
                BaseType.ReplaceLastToken(newToken));
        }
    }
}