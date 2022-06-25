// ------------------------------------------------------------------------------
// <copyright file="VJassStatementElseClauseSyntax.cs" company="Drake53">
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
    public class VJassStatementElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassStatementElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassStatementSyntax> statements)
        {
            ElseToken = elseToken;
            Statements = statements;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStatementElseClauseSyntax statementElseClause
                && Statements.IsEquivalentTo(statementElseClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseToken.ProcessTo(writer, context);
            Statements.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Statements.IsEmpty ? ElseToken : Statements[^1].GetLastToken();

        protected internal override VJassStatementElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStatementElseClauseSyntax(
                newToken,
                Statements);
        }

        protected internal override VJassStatementElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new VJassStatementElseClauseSyntax(
                    ElseToken,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new VJassStatementElseClauseSyntax(
                newToken,
                Statements);
        }
    }
}