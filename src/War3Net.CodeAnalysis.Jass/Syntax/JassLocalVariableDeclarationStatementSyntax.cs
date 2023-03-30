// ------------------------------------------------------------------------------
// <copyright file="JassLocalVariableDeclarationStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassLocalVariableDeclarationStatementSyntax : JassStatementSyntax
    {
        internal JassLocalVariableDeclarationStatementSyntax(
            JassSyntaxToken localToken,
            JassVariableOrArrayDeclaratorSyntax declarator)
        {
            LocalToken = localToken;
            Declarator = declarator;
        }

        public JassSyntaxToken LocalToken { get; }

        public JassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxFacts.GetLocalDeclarationStatementKind(Declarator.SyntaxKind);

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement
                && Declarator.IsEquivalentTo(localVariableDeclarationStatement.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            LocalToken.WriteTo(writer);
            Declarator.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return Declarator;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return LocalToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return LocalToken;
            yield return Declarator;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return Declarator;
            foreach (var descendant in Declarator.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return LocalToken;

            foreach (var descendant in Declarator.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return LocalToken;

            yield return Declarator;
            foreach (var descendant in Declarator.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{LocalToken} {Declarator}";

        public override JassSyntaxToken GetFirstToken() => LocalToken;

        public override JassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override JassLocalVariableDeclarationStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                newToken,
                Declarator);
        }

        protected internal override JassLocalVariableDeclarationStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                LocalToken,
                Declarator.ReplaceLastToken(newToken));
        }
    }
}