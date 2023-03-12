// ------------------------------------------------------------------------------
// <copyright file="JassInvocationExpressionSyntax.cs" company="Drake53">
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
    public class JassInvocationExpressionSyntax : JassExpressionSyntax
    {
        internal JassInvocationExpressionSyntax(
            JassIdentifierNameSyntax identifierName,
            JassArgumentListSyntax argumentList)
        {
            IdentifierName = identifierName;
            ArgumentList = argumentList;
        }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassInvocationExpressionSyntax invocationExpression
                && IdentifierName.IsEquivalentTo(invocationExpression.IdentifierName)
                && ArgumentList.IsEquivalentTo(invocationExpression.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            IdentifierName.WriteTo(writer);
            ArgumentList.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;
            yield return ArgumentList;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            yield return IdentifierName;
            yield return ArgumentList;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return ArgumentList;
            foreach (var descendant in ArgumentList.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ArgumentList.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ArgumentList;
            foreach (var descendant in ArgumentList.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{IdentifierName}{ArgumentList}";

        public override JassSyntaxToken GetFirstToken() => IdentifierName.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override JassInvocationExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassInvocationExpressionSyntax(
                IdentifierName.ReplaceFirstToken(newToken),
                ArgumentList);
        }

        protected internal override JassInvocationExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassInvocationExpressionSyntax(
                IdentifierName,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}