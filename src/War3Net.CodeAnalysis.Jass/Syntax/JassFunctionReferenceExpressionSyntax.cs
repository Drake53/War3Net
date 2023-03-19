// ------------------------------------------------------------------------------
// <copyright file="JassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionReferenceExpressionSyntax : JassExpressionSyntax
    {
        internal JassFunctionReferenceExpressionSyntax(
            JassSyntaxToken functionToken,
            JassIdentifierNameSyntax identifierName)
        {
            FunctionToken = functionToken;
            IdentifierName = identifierName;
        }

        public JassSyntaxToken FunctionToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassFunctionReferenceExpressionSyntax functionReferenceExpression
                && IdentifierName.IsEquivalentTo(functionReferenceExpression.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            FunctionToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return FunctionToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return FunctionToken;
            yield return IdentifierName;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return FunctionToken;

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return FunctionToken;

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{FunctionToken} {IdentifierName}";

        public override JassSyntaxToken GetFirstToken() => FunctionToken;

        public override JassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override JassFunctionReferenceExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassFunctionReferenceExpressionSyntax(
                newToken,
                IdentifierName);
        }

        protected internal override JassFunctionReferenceExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassFunctionReferenceExpressionSyntax(
                FunctionToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}