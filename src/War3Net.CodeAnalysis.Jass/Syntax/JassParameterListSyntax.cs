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
            SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> parameterList)
        {
            TakesToken = takesToken;
            ParameterList = parameterList;
        }

        public JassSyntaxToken TakesToken { get; }

        public SeparatedSyntaxList<JassParameterSyntax, JassSyntaxToken> ParameterList { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.ParameterList;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassParameterListSyntax parameterList
                && ParameterList.IsEquivalentTo(parameterList.ParameterList);
        }

        public override void WriteTo(TextWriter writer)
        {
            TakesToken.WriteTo(writer);
            ParameterList.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return ParameterList.Items;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return TakesToken;

            foreach (var child in ParameterList.Separators)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return TakesToken;

            foreach (var child in ParameterList.GetChildNodesAndTokens())
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return ParameterList.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return TakesToken;

            foreach (var descendant in ParameterList.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return TakesToken;

            foreach (var descendant in ParameterList.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{TakesToken} {ParameterList}";

        public override JassSyntaxToken GetFirstToken() => TakesToken;

        public override JassSyntaxToken GetLastToken() => ParameterList.Items[^1].GetLastToken();

        protected internal override JassParameterListSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassParameterListSyntax(
                newToken,
                ParameterList);
        }

        protected internal override JassParameterListSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassParameterListSyntax(
                TakesToken,
                ParameterList.ReplaceLastItem(ParameterList.Items[^1].ReplaceLastToken(newToken)));
        }
    }
}