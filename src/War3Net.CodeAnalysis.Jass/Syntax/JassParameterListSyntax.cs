// ------------------------------------------------------------------------------
// <copyright file="JassParameterListSyntax.cs" company="Drake53">
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
    public class JassParameterListSyntax : JassParameterListOrEmptyParameterListSyntax
    {
        internal JassParameterListSyntax(
            JassSyntaxToken takesToken,
            SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> parameters)
        {
            TakesToken = takesToken;
            Parameters = parameters;
        }

        public override JassSyntaxToken TakesToken { get; }

        public override SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> Parameters { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ParameterList;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassParameterListSyntax parameterList
                && Parameters.IsEquivalentTo(parameterList.Parameters);
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            Parameters.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return Parameters.Items;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return TakesToken;

            foreach (var child in Parameters.Separators)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return TakesToken;

            foreach (var child in Parameters.GetChildNodesAndTokens())
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return Parameters.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return TakesToken;

            foreach (var descendant in Parameters.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return TakesToken;

            foreach (var descendant in Parameters.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{TakesToken} {Parameters}";

        public override JassSyntaxToken GetFirstToken() => TakesToken;

        public override JassSyntaxToken GetLastToken() => Parameters.Items[^1].GetLastToken();

        public override void Accept(IJassSyntaxVisitor visitor) => visitor.VisitParameterList(this);

        public override TResult? Accept<TResult>(IJassSyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitParameterList(this);

        public JassParameterListSyntax Update(
            JassSyntaxToken takesToken,
            SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> parameters)
        {
            if (ReferenceEquals(TakesToken, takesToken) &&
                ReferenceEquals(Parameters, parameters))
            {
                return this;
            }

            ThrowHelper.ThrowIfInvalidToken(takesToken, JassSyntaxKind.TakesKeyword);

            return new JassParameterListSyntax(takesToken, parameters);
        }

        public JassParameterListSyntax WithTakesToken(JassSyntaxToken takesToken) => Update(takesToken, Parameters);

        public JassParameterListSyntax WithParameters(SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> parameters) => Update(TakesToken, parameters);

        protected internal override JassParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassParameterListSyntax(
                newToken,
                Parameters);
        }

        protected internal override JassParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassParameterListSyntax(
                TakesToken,
                Parameters.ReplaceLastItem(Parameters.Items[^1].ReplaceLastToken(newToken)));
        }
    }
}