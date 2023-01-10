// ------------------------------------------------------------------------------
// <copyright file="JassElseIfClauseSyntax.cs" company="Drake53">
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
    public class JassElseIfClauseSyntax : JassSyntaxNode
    {
        internal JassElseIfClauseSyntax(
            JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<JassStatementSyntax> statements)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Statements = statements;
        }

        public JassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElseIfClauseSyntax statementElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(statementElseIfClause.ElseIfClauseDeclarator)
                && Statements.IsEquivalentTo(statementElseIfClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override JassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Statements.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        protected internal override JassElseIfClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override JassElseIfClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new JassElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new JassElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}