// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalDeclarationElseClauseSyntax.cs" company="Drake53">
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
    public class VJassGlobalDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassGlobalDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassGlobalDeclarationSyntax> globals)
        {
            ElseToken = elseToken;
            Globals = globals;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassGlobalDeclarationSyntax> Globals { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalDeclarationElseClauseSyntax globalDeclarationElseClause
                && Globals.IsEquivalentTo(globalDeclarationElseClause.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Globals.WriteTo(writer);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Globals.IsEmpty ? ElseToken : Globals[^1].GetLastToken();

        protected internal override VJassGlobalDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }

        protected internal override VJassGlobalDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Globals.IsEmpty)
            {
                return new VJassGlobalDeclarationElseClauseSyntax(
                    ElseToken,
                    Globals.ReplaceLastItem(Globals[^1].ReplaceLastToken(newToken)));
            }

            return new VJassGlobalDeclarationElseClauseSyntax(
                newToken,
                Globals);
        }
    }
}