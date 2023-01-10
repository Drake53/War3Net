// ------------------------------------------------------------------------------
// <copyright file="JassIfStatementSyntax.cs" company="Drake53">
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
    public class JassIfStatementSyntax : JassStatementSyntax
    {
        internal JassIfStatementSyntax(
            JassIfClauseSyntax ifClause,
            ImmutableArray<JassElseIfClauseSyntax> elseIfClauses,
            JassElseClauseSyntax? elseClause,
            JassSyntaxToken endIfToken)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public JassIfClauseSyntax IfClause { get; }

        public ImmutableArray<JassElseIfClauseSyntax> ElseIfClauses { get; }

        public JassElseClauseSyntax? ElseClause { get; }

        public JassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassIfStatementSyntax ifStatement
                && IfClause.IsEquivalentTo(ifStatement.IfClause)
                && ElseIfClauses.IsEquivalentTo(ifStatement.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(ifStatement.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override string ToString() => IfClause.ToString();

        public override JassSyntaxToken GetFirstToken() => IfClause.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override JassIfStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassIfStatementSyntax(
                IfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override JassIfStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassIfStatementSyntax(
                IfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}