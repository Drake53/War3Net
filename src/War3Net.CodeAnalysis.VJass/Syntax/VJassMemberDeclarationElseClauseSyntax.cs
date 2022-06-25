// ------------------------------------------------------------------------------
// <copyright file="VJassMemberDeclarationElseClauseSyntax.cs" company="Drake53">
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
    public class VJassMemberDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassMemberDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassMemberDeclarationSyntax> memberDeclarations)
        {
            ElseToken = elseToken;
            MemberDeclarations = memberDeclarations;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassMemberDeclarationSyntax> MemberDeclarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMemberDeclarationElseClauseSyntax memberDeclarationElseClause
                && MemberDeclarations.IsEquivalentTo(memberDeclarationElseClause.MemberDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            MemberDeclarations.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseToken.ProcessTo(writer, context);
            MemberDeclarations.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => MemberDeclarations.IsEmpty ? ElseToken : MemberDeclarations[^1].GetLastToken();

        protected internal override VJassMemberDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMemberDeclarationElseClauseSyntax(
                newToken,
                MemberDeclarations);
        }

        protected internal override VJassMemberDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!MemberDeclarations.IsEmpty)
            {
                return new VJassMemberDeclarationElseClauseSyntax(
                    ElseToken,
                    MemberDeclarations.ReplaceLastItem(MemberDeclarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassMemberDeclarationElseClauseSyntax(
                newToken,
                MemberDeclarations);
        }
    }
}