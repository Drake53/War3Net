// ------------------------------------------------------------------------------
// <copyright file="VJassStatementIfClauseSyntax.cs" company="Drake53">
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
    public class VJassStatementIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassStatementIfClauseSyntax(
            VJassIfClauseDeclaratorSyntax ifClauseDeclarator,
            ImmutableArray<VJassStatementSyntax> statements)
        {
            IfClauseDeclarator = ifClauseDeclarator;
            Statements = statements;
        }

        public VJassIfClauseDeclaratorSyntax IfClauseDeclarator { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStatementIfClauseSyntax statementIfClause
                && IfClauseDeclarator.IsEquivalentTo(statementIfClause.IfClauseDeclarator)
                && Statements.IsEquivalentTo(statementIfClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClauseDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            IfClauseDeclarator.ProcessTo(writer, context);
            Statements.ProcessTo(writer, context);
        }

        public override string ToString() => $"{IfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => IfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Statements.IsEmpty ? IfClauseDeclarator.GetLastToken() : Statements[^1].GetLastToken();

        protected internal override VJassStatementIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStatementIfClauseSyntax(
                IfClauseDeclarator.ReplaceFirstToken(newToken),
                Statements);
        }

        protected internal override VJassStatementIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new VJassStatementIfClauseSyntax(
                    IfClauseDeclarator,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new VJassStatementIfClauseSyntax(
                IfClauseDeclarator.ReplaceLastToken(newToken),
                Statements);
        }
    }
}