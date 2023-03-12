// ------------------------------------------------------------------------------
// <copyright file="JassReturnClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassReturnClauseSyntax : JassSyntaxNode
    {
        internal JassReturnClauseSyntax(
            JassSyntaxToken returnsToken,
            JassTypeSyntax returnType)
        {
            ReturnsToken = returnsToken;
            ReturnType = returnType;
        }

        public JassSyntaxToken ReturnsToken { get; }

        public JassTypeSyntax ReturnType { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassReturnClauseSyntax returnClause
                && ReturnType.IsEquivalentTo(returnClause.ReturnType);
        }

        public override void WriteTo(TextWriter writer)
        {
            ReturnsToken.WriteTo(writer);
            ReturnType.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return ReturnType;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return ReturnsToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return ReturnsToken;
            yield return ReturnType;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return ReturnType;
            foreach (var descendant in ReturnType.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return ReturnsToken;

            foreach (var descendant in ReturnType.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return ReturnsToken;

            yield return ReturnType;
            foreach (var descendant in ReturnType.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{ReturnsToken} {ReturnType}";

        public override JassSyntaxToken GetFirstToken() => ReturnsToken;

        public override JassSyntaxToken GetLastToken() => ReturnType.GetLastToken();

        protected internal override JassReturnClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassReturnClauseSyntax(
                newToken,
                ReturnType);
        }

        protected internal override JassReturnClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassReturnClauseSyntax(
                ReturnsToken,
                ReturnType.ReplaceLastToken(newToken));
        }
    }
}