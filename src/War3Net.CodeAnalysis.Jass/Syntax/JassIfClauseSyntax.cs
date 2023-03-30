// ------------------------------------------------------------------------------
// <copyright file="JassIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassIfClauseSyntax : JassSyntaxNode
    {
        internal JassIfClauseSyntax(
            JassIfClauseDeclaratorSyntax ifClauseDeclarator,
            ImmutableArray<JassStatementSyntax> statements)
        {
            IfClauseDeclarator = ifClauseDeclarator;
            Statements = statements;
        }

        public JassIfClauseDeclaratorSyntax IfClauseDeclarator { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.IfClause;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIfClauseSyntax ifClause
                && IfClauseDeclarator.IsEquivalentTo(ifClause.IfClauseDeclarator)
                && Statements.IsEquivalentTo(ifClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IfClauseDeclarator;
            foreach (var child in Statements)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield break;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return IfClauseDeclarator;
            foreach (var child in Statements)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IfClauseDeclarator;
            foreach (var descendant in IfClauseDeclarator.GetDescendantNodes())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in IfClauseDeclarator.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return IfClauseDeclarator;
            foreach (var descendant in IfClauseDeclarator.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
        }

        public override string ToString() => $"{IfClauseDeclarator} [...]";

        public override JassSyntaxToken GetFirstToken() => IfClauseDeclarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Statements.IsEmpty ? IfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        protected internal override JassIfClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIfClauseSyntax(
                IfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override JassIfClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new JassIfClauseSyntax(
                    IfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new JassIfClauseSyntax(
                IfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}