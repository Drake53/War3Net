// ------------------------------------------------------------------------------
// <copyright file="VJassTopLevelDeclarationElseClauseSyntax.cs" company="Drake53">
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
    public class VJassTopLevelDeclarationElseClauseSyntax : VJassSyntaxNode
    {
        internal VJassTopLevelDeclarationElseClauseSyntax(
            VJassSyntaxToken elseToken,
            ImmutableArray<VJassTopLevelDeclarationSyntax> declarations)
        {
            ElseToken = elseToken;
            Declarations = declarations;
        }

        public VJassSyntaxToken ElseToken { get; }

        public ImmutableArray<VJassTopLevelDeclarationSyntax> Declarations { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTopLevelDeclarationElseClauseSyntax topLevelDeclarationElseClause
                && Declarations.IsEquivalentTo(topLevelDeclarationElseClause.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            ElseToken.WriteTo(writer);
            Declarations.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ElseToken.ProcessTo(writer, context);
            Declarations.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ElseToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => ElseToken;

        public override VJassSyntaxToken GetLastToken() => Declarations.IsEmpty ? ElseToken : Declarations[^1].GetLastToken();

        protected internal override VJassTopLevelDeclarationElseClauseSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTopLevelDeclarationElseClauseSyntax(
                newToken,
                Declarations);
        }

        protected internal override VJassTopLevelDeclarationElseClauseSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassTopLevelDeclarationElseClauseSyntax(
                    ElseToken,
                    Declarations.ReplaceLastItem(Declarations[^1].ReplaceLastToken(newToken)));
            }

            return new VJassTopLevelDeclarationElseClauseSyntax(
                newToken,
                Declarations);
        }
    }
}