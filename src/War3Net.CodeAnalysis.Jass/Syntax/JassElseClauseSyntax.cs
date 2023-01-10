// ------------------------------------------------------------------------------
// <copyright file="JassElseClauseSyntax.cs" company="Drake53">
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
    public class JassElseClauseSyntax : JassSyntaxNode
    {
        internal JassElseClauseSyntax(
            JassSyntaxToken elseToken,
            ImmutableArray<JassStatementSyntax> statements)
        {
            ElseToken = elseToken;
            Statements = statements;
        }

        public JassSyntaxToken ElseToken { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassElseClauseSyntax statementElseClause
                && Statements.IsEquivalentTo(statementElseClause.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Statements.WriteTo(writer);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override JassSyntaxToken GetFirstToken() => ElseToken;

        public override JassSyntaxToken GetLastToken() => Statements.IsEmpty ? ElseToken : Statements[^1].GetLastToken();

        protected internal override JassElseClauseSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassElseClauseSyntax(
                newToken,
                Statements);
        }

        protected internal override JassElseClauseSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (!Statements.IsEmpty)
            {
                return new JassElseClauseSyntax(
                    ElseToken,
                    Statements.ReplaceLastItem(Statements[^1].ReplaceLastToken(newToken)));
            }

            return new JassElseClauseSyntax(
                newToken,
                Statements);
        }
    }
}