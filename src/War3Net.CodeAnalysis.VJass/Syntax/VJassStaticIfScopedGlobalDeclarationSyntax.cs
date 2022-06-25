// ------------------------------------------------------------------------------
// <copyright file="VJassStaticIfScopedGlobalDeclarationSyntax.cs" company="Drake53">
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
    public class VJassStaticIfScopedGlobalDeclarationSyntax : VJassScopedGlobalDeclarationSyntax
    {
        internal VJassStaticIfScopedGlobalDeclarationSyntax(
            VJassScopedGlobalDeclarationStaticIfClauseSyntax staticIfClause,
            ImmutableArray<VJassScopedGlobalDeclarationElseIfClauseSyntax> elseIfClauses,
            VJassScopedGlobalDeclarationElseClauseSyntax? elseClause,
            VJassSyntaxToken endIfToken)
        {
            StaticIfClause = staticIfClause;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
            EndIfToken = endIfToken;
        }

        public VJassScopedGlobalDeclarationStaticIfClauseSyntax StaticIfClause { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationElseIfClauseSyntax> ElseIfClauses { get; }

        public VJassScopedGlobalDeclarationElseClauseSyntax? ElseClause { get; }

        public VJassSyntaxToken EndIfToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStaticIfScopedGlobalDeclarationSyntax staticIfScopedGlobalDeclaration
                && StaticIfClause.IsEquivalentTo(staticIfScopedGlobalDeclaration.StaticIfClause)
                && ElseIfClauses.IsEquivalentTo(staticIfScopedGlobalDeclaration.ElseIfClauses)
                && ElseClause.NullableEquivalentTo(staticIfScopedGlobalDeclaration.ElseClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            StaticIfClause.WriteTo(writer);
            ElseIfClauses.WriteTo(writer);
            ElseClause?.WriteTo(writer);
            EndIfToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            StaticIfClause.ProcessTo(writer, context);
            ElseIfClauses.ProcessTo(writer, context);
            ElseClause?.ProcessTo(writer, context);
            EndIfToken.ProcessTo(writer, context);
        }

        public override string ToString() => StaticIfClause.ToString();

        public override VJassSyntaxToken GetFirstToken() => StaticIfClause.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndIfToken;

        protected internal override VJassStaticIfScopedGlobalDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfScopedGlobalDeclarationSyntax(
                StaticIfClause.ReplaceFirstToken(newToken),
                ElseIfClauses,
                ElseClause,
                EndIfToken);
        }

        protected internal override VJassStaticIfScopedGlobalDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassStaticIfScopedGlobalDeclarationSyntax(
                StaticIfClause,
                ElseIfClauses,
                ElseClause,
                newToken);
        }
    }
}