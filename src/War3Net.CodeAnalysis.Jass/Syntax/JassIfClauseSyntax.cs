// ------------------------------------------------------------------------------
// <copyright file="JassIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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