// ------------------------------------------------------------------------------
// <copyright file="JassCallStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCallStatementSyntax : JassStatementSyntax
    {
        internal JassCallStatementSyntax(
            JassSyntaxToken callToken,
            JassIdentifierNameSyntax identifierName,
            JassArgumentListSyntax argumentList)
        {
            CallToken = callToken;
            IdentifierName = identifierName;
            ArgumentList = argumentList;
        }

        public JassSyntaxToken CallToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassArgumentListSyntax ArgumentList { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassCallStatementSyntax callStatement
                && IdentifierName.IsEquivalentTo(callStatement.IdentifierName)
                && ArgumentList.IsEquivalentTo(callStatement.ArgumentList);
        }

        public override void WriteTo(TextWriter writer)
        {
            CallToken.WriteTo(writer);
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
            yield return CallToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return CallToken;
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
            yield return CallToken;

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ArgumentList.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return CallToken;

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

        public override string ToString() => $"{CallToken} {IdentifierName}{ArgumentList}";

        public override JassSyntaxToken GetFirstToken() => CallToken;

        public override JassSyntaxToken GetLastToken() => ArgumentList.GetLastToken();

        protected internal override JassCallStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassCallStatementSyntax(
                newToken,
                IdentifierName,
                ArgumentList);
        }

        protected internal override JassCallStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassCallStatementSyntax(
                CallToken,
                IdentifierName,
                ArgumentList.ReplaceLastToken(newToken));
        }
    }
}