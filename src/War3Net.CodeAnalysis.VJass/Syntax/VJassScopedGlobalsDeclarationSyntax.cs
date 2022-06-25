// ------------------------------------------------------------------------------
// <copyright file="VJassScopedGlobalsDeclarationSyntax.cs" company="Drake53">
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
    public class VJassScopedGlobalsDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassScopedGlobalsDeclarationSyntax(
            VJassSyntaxToken globalsToken,
            ImmutableArray<VJassScopedGlobalDeclarationSyntax> globals,
            VJassSyntaxToken endGlobalsToken)
        {
            GlobalsToken = globalsToken;
            Globals = globals;
            EndGlobalsToken = endGlobalsToken;
        }

        public VJassSyntaxToken GlobalsToken { get; }

        public ImmutableArray<VJassScopedGlobalDeclarationSyntax> Globals { get; }

        public VJassSyntaxToken EndGlobalsToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopedGlobalsDeclarationSyntax scopedGlobalsDeclaration
                && Globals.IsEquivalentTo(scopedGlobalsDeclaration.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            GlobalsToken.WriteTo(writer);
            Globals.WriteTo(writer);
            EndGlobalsToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            GlobalsToken.ProcessTo(writer, context);
            Globals.ProcessTo(writer, context);
            EndGlobalsToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{GlobalsToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => GlobalsToken;

        public override VJassSyntaxToken GetLastToken() => EndGlobalsToken;

        protected internal override VJassScopedGlobalsDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalsDeclarationSyntax(
                newToken,
                Globals,
                EndGlobalsToken);
        }

        protected internal override VJassScopedGlobalsDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassScopedGlobalsDeclarationSyntax(
                GlobalsToken,
                Globals,
                newToken);
        }
    }
}