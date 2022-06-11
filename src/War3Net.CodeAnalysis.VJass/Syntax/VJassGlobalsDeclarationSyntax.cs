// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalsDeclarationSyntax.cs" company="Drake53">
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
    public class VJassGlobalsDeclarationSyntax : VJassTopLevelDeclarationSyntax
    {
        internal VJassGlobalsDeclarationSyntax(
            VJassSyntaxToken globalsToken,
            ImmutableArray<VJassGlobalDeclarationSyntax> globals,
            VJassSyntaxToken endGlobalsToken)
        {
            GlobalsToken = globalsToken;
            Globals = globals;
            EndGlobalsToken = endGlobalsToken;
        }

        public VJassSyntaxToken GlobalsToken { get; }

        public ImmutableArray<VJassGlobalDeclarationSyntax> Globals { get; }

        public VJassSyntaxToken EndGlobalsToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalsDeclarationSyntax globalsDeclaration
                && Globals.IsEquivalentTo(globalsDeclaration.Globals);
        }

        public override void WriteTo(TextWriter writer)
        {
            GlobalsToken.WriteTo(writer);
            Globals.WriteTo(writer);
            EndGlobalsToken.WriteTo(writer);
        }

        public override string ToString() => $"{GlobalsToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => GlobalsToken;

        public override VJassSyntaxToken GetLastToken() => EndGlobalsToken;

        protected internal override VJassGlobalsDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalsDeclarationSyntax(
                newToken,
                Globals,
                EndGlobalsToken);
        }

        protected internal override VJassGlobalsDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalsDeclarationSyntax(
                GlobalsToken,
                Globals,
                newToken);
        }
    }
}