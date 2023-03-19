// ------------------------------------------------------------------------------
// <copyright file="JassArrayDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassArrayDeclaratorSyntax : JassVariableOrArrayDeclaratorSyntax
    {
        internal JassArrayDeclaratorSyntax(
            JassTypeSyntax type,
            JassSyntaxToken arrayToken,
            JassIdentifierNameSyntax identifierName)
        {
            Type = type;
            ArrayToken = arrayToken;
            IdentifierName = identifierName;
        }

        public JassTypeSyntax Type { get; }

        public JassSyntaxToken ArrayToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassArrayDeclaratorSyntax arrayDeclarator
                && Type.IsEquivalentTo(arrayDeclarator.Type)
                && IdentifierName.IsEquivalentTo(arrayDeclarator.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            ArrayToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Type;
            yield return IdentifierName;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ArrayToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Type;
            yield return ArrayToken;
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

            yield return ArrayToken;

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

            yield return ArrayToken;

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{Type} {ArrayToken} {IdentifierName}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override JassArrayDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassArrayDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                ArrayToken,
                IdentifierName);
        }

        protected internal override JassArrayDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassArrayDeclaratorSyntax(
                Type,
                ArrayToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}