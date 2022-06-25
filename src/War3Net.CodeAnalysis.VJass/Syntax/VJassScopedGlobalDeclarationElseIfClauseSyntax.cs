// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationElseIfClauseSyntax.cs" company="Drake53">
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
    public class VJassScopedGlobalDeclarationElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedGlobalDeclarationElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassScopedGlobalDeclarationSyntax> globals)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            Globals = globals;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedGlobalDeclarationElseIfClauseSyntax scopedGlobalDeclarationElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(scopedGlobalDeclarationElseIfClause.ElseIfClauseDeclarator)
                && Globals.IsEquivalentTo(scopedGlobalDeclarationElseIfClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseIfClauseDeclarator.ProcessTo(writer, context);
            Globals.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : Globals[^1].GetLastToken();

        protected internal override VJassScopedGlobalDeclarationElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                Globals);
        }

        protected internal override VJassScopedGlobalDeclarationElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassScopedGlobalDeclarationElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedGlobalDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                Globals);
        }
    }
}