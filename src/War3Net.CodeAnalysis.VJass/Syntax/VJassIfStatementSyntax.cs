// ------------------------------------------------------------------------------
// <copyright file="VJassIfStatementSyntax.cs" company="Drake53">
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
    public class VJassIfStatementSyntax : VJassStatementSyntax
    {
        internal VJassIfStatementSyntax(
            VJassStatementIfClauseSyntax ifClause,
            ImmutableArray<VJassStatementElseIfClauseSyntax> elseIfClauses,
            VJassStatementElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            IfClause = ifClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassStatementIfClauseSyntax IfClause { get; }

        public ImmutableArray<VJassStatementElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassStatementElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassIfStatementSyntax ifStatement
                && IfClause.IsEquivalentTo(ifStatement.IfClause)
                && ElseIfClauses.IsEquivalentTo(ifStatement.ElseIfClauses)
                && ElseClause.NullableEquals(ifStatement.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            IfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override string ToString() => IfClause.ToString();

        public override VJassSyntaxToken GetFirstToken() => IfClause.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override VJassIfStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassIfStatementSyntax(
                IfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassIfStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassIfStatementSyntax(
                IfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}