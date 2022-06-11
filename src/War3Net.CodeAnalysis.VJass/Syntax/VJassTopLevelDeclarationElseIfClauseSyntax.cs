// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationElseIfClauseSyntax.cs" company="Drake53">
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
    public class VJassTopLevelDeclarationElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassTopLevelDeclarationElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassTopLevelDeclarationSyntax> declarations)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Declarations = declarations;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassTopLevelDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTopLevelDeclarationElseIfClauseSyntax topLevelDeclarationElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(topLevelDeclarationElseIfClause.ElseIfClauseDeclarator)
                && Declarations.IsEquivalentTo(topLevelDeclarationElseIfClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Declarations[^1].GetLastToken();

        protected internal override VJassTopLevelDeclarationElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTopLevelDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Declarations);
        }

        protected internal override VJassTopLevelDeclarationElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassTopLevelDeclarationElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassTopLevelDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Declarations);
        }
    }
}