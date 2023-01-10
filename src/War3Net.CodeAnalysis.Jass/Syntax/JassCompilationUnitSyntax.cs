// ------------------------------------------------------------------------------
// <copyright file="JassCompilationUnitSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassCompilationUnitSyntax : JassSyntaxNode
    {
        internal JassCompilationUnitSyntax(
            ImmutableArray<JassTopLevelDeclarationSyntax> declarations,
            JassSyntaxToken endOfFileToken)
        {
            Declarations = declarations;
            EndOfFileToken = endOfFileToken;
        }

        public ImmutableArray<JassTopLevelDeclarationSyntax> Declarations { get; }

        public JassSyntaxToken EndOfFileToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassCompilationUnitSyntax compilationUnit
                && Declarations.IsEquivalentTo(compilationUnit.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarations.WriteTo(writer);
            EndOfFileToken.WriteTo(writer);
        }

        public override JassSyntaxToken GetFirstToken() => Declarations.IsEmpty ? EndOfFileToken : Declarations[0].GetFirstToken();

        public override JassSyntaxToken GetLastToken() => EndOfFileToken;

        protected internal override JassCompilationUnitSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            if (!Declarations.IsEmpty)
            {
                return new JassCompilationUnitSyntax(
                    Declarations.ReplaceFirstItem(Declarations[0].ReplaceFirstToken(newToken)),
                    EndOfFileToken);
            }

            return new JassCompilationUnitSyntax(
                Declarations,
                newToken);
        }

        protected internal override JassCompilationUnitSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassCompilationUnitSyntax(
                Declarations,
                newToken);
        }
    }
}