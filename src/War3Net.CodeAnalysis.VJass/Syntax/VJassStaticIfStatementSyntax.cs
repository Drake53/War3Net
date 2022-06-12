// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfStatementSyntax.cs" company="Drake53">
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
    public class VJassStaticIfStatementSyntax : VJassStatementSyntax
    {
        internal VJassStaticIfStatementSyntax(
            VJassStatementStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassStatementElseIfClauseSyntax> elseIfClauses,
            VJassStatementElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassStatementStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassStatementElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassStatementElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfStatementSyntax staticIfStatement
                && StaticIfClause.IsEquivalentTo(staticIfStatement.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfStatement.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(staticIfStatement.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override string ToString() => StaticIfClause.ToString();

        public override VJassSyntaxToken GetFirstToken() => StaticIfClause.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override VJassStaticIfStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfStatementSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfStatementSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}