// ------------------------------------------------------------------------------
// <copyright file="VJassStatementElseIfClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStatementElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassStatementElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassStatementSyntax> statements)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Statements = statements;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStatementElseIfClauseSyntax statementElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(statementElseIfClause.ElseIfClauseDeclarator)
                && Statements.IsEquivalentTo(statementElseIfClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Statements.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        protected internal override VJassStatementElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStatementElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override VJassStatementElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new VJassStatementElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new VJassStatementElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}