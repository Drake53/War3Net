// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationElseIfClauseSyntax.cs" company="Drake53">
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
    public class VJassGlobalDeclarationElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassGlobalDeclarationElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassGlobalDeclarationSyntax> globals)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Globals = globals;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalDeclarationElseIfClauseSyntax globalDeclarationElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(globalDeclarationElseIfClause.ElseIfClauseDeclarator)
                && Globals.IsEquivalentTo(globalDeclarationElseIfClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Globals[^1].GetLastToken();

        protected internal override VJassGlobalDeclarationElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Globals);
        }

        protected internal override VJassGlobalDeclarationElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassGlobalDeclarationElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassGlobalDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Globals);
        }
    }
}