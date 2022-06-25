// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalDeclarationElseClauseSyntax.cs" company="Drake53">
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
    public class VJassScopedGlobalDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassScopedGlobalDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassScopedGlobalDeclarationSyntax> globals)
        {
            ElseToken = elseToken;
            Globals = globals;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedGlobalDeclarationElseClauseSyntax scopedGlobalDeclarationElseClause
                && Globals.IsEquivalentTo(scopedGlobalDeclarationElseClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseToken.ProcessTo(writer, context);
            Globals.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? ElseToken : Globals[^1].GetLastToken();

        protected internal override VJassScopedGlobalDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }

        protected internal override VJassScopedGlobalDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassScopedGlobalDeclarationElseClauseSyntax(
                    ElseToken,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassScopedGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }
    }
}