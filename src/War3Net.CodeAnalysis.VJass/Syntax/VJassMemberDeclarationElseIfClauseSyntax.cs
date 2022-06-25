// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationElseIfClauseSyntax.cs" company="Drake53">
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
    public class VJassMemberDeclarationElseIfClauseSyntax : VJassSyntaxNode
    {
        internal VJassMemberDeclarationElseIfClauseSyntax(
            VJassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator,
            ImmutableArray<VJassMemberDeclarationSyntax> memberDeclarations)
        {
            ElseIfClauseDeclarator = elseIfClauseDeclarator;
            MemberDeclarations = memberDeclarations;
        }

        public VJassElseIfClauseDeclaratorSyntax ElseIfClauseDeclarator { get; }

        public ImmutableArray<VJassMemberDeclarationSyntax> MemberDeclarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMemberDeclarationElseIfClauseSyntax memberDeclarationElseIfClause
                && ElseIfClauseDeclarator.IsEquivalentTo(memberDeclarationElseIfClause.ElseIfClauseDeclarator)
                && MemberDeclarations.IsEquivalentTo(memberDeclarationElseIfClause.MemberDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseIfClauseDeclarator.WriteTo(writer);
            MemberDeclarations.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseIfClauseDeclarator.ProcessTo(writer, context);
            MemberDeclarations.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseIfClauseDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseIfClauseDeclarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => MemberDeclarations.IsEmpty ? ElseIfClauseDeclarator.GetLastToken() : MemberDeclarations[^1].GetLastToken();

        protected internal override VJassMemberDeclarationElseIfClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceFirstToken(newToken),
                MemberDeclarations);
        }

        protected internal override VJassMemberDeclarationElseIfClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!MemberDeclarations.IsEmpty)
            {
                return new VJassMemberDeclarationElseIfClauseSyntax(
                    ElseIfClauseDeclarator,
                    MemberDeclarations.ReplaceLastItem(MemberDeclarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassMemberDeclarationElseIfClauseSyntax(
                ElseIfClauseDeclarator.ReplaceLastToken(newToken),
                MemberDeclarations);
        }
    }
}