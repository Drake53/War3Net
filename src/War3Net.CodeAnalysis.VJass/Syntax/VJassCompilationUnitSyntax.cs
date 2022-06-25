// ------------------------------------------------------------------------------
// <copyright file="VJassCompilationUnitSyntax.cs" company="Drake53">
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
    public sealed class VJassCompilationUnitSyntax : VJassSyntaxNode
    {
        internal VJassCompilationUnitSyntax(
            ImmutableArray<VJassTopLevelDeclarationSyntax> declarations,
            VJassSyntaxToken endOfFileToken)
        {
            Declarations = declarations;
            EndOfFileToken = endOfFileToken;
        }

        public ImmutableArray<VJassTopLevelDeclarationSyntax> Declarations { get; }

        public VJassSyntaxToken EndOfFileToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassCompilationUnitSyntax compilationUnit
                && Declarations.IsEquivalentTo(compilationUnit.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarations.WriteTo(writer);
            EndOfFileToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Declarations.ProcessTo(writer, context);
            EndOfFileToken.ProcessTo(writer, context);
        }

        public override VJassSyntaxToken GetFirstToken() => Declarations.IsEmpty ? EndOfFileToken : Declarations[0].GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndOfFileToken;

        protected internal override VJassCompilationUnitSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new VJassCompilationUnitSyntax(
                    Declarations.ReplaceFirstItem(Declarations[0].ReplaceFirstToken(newToken)),
                    EndOfFileToken);
            }

            return new VJassCompilationUnitSyntax(
                Declarations,
                newToken);
        }

        protected internal override VJassCompilationUnitSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassCompilationUnitSyntax(
                Declarations,
                newToken);
        }
    }
}