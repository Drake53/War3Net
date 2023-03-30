// ------------------------------------------------------------------------------
// <copyright file="JassIdentifierNameSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIdentifierNameSyntax : JassExpressionSyntax
    {
        internal JassIdentifierNameSyntax(
            JassSyntaxToken token)
        {
            Token = token;
        }

        public JassSyntaxToken Token { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IdentifierName;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIdentifierNameSyntax identifierName
                && Token.IsEquivalentTo(identifierName.Token);
        }

        public override void WriteTo(TextWriter writer)
        {
            Token.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return Token;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return Token;
        }

        public override string ToString() => Token.ToString();

        public override JassSyntaxToken GetFirstToken() => Token;

        public override JassSyntaxToken GetLastToken() => Token;

        protected internal override JassIdentifierNameSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }

        protected internal override JassIdentifierNameSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIdentifierNameSyntax(newToken);
        }
    }
}