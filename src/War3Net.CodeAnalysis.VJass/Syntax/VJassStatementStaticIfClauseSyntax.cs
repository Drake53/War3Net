// ------------------------------------------------------------------------------
// <copyright file="VJassStatementStaticIfClauseSyntax.cs" company="Drake53">
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
    public class VJassStatementStaticIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassStatementStaticIfClauseSyntax(
            VJassStaticIfClauseDeclaratorSyntax staticIfClauseDeclarator,
            ImmutableArray<VJassStatementSyntax> statements)
        {
            StaticIfClauseDeclarator = staticIfClauseDeclarator;
            Statements = statements;
        }

        public VJassStaticIfClauseDeclaratorSyntax StaticIfClauseDeclarator { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStatementStaticIfClauseSyntax statementStaticIfClause
                && StaticIfClauseDeclarator.IsEquivalentTo(statementStaticIfClause.StaticIfClauseDeclarator)
                && Statements.IsEquivalentTo(statementStaticIfClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override string ToString() => $"{StaticIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => StaticIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Statements.IsEmpty ? StaticIfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        protected internal override VJassStatementStaticIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStatementStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override VJassStatementStaticIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new VJassStatementStaticIfClauseSyntax(
                    StaticIfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new VJassStatementStaticIfClauseSyntax(
                StaticIfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}