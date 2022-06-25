// ------------------------------------------------------------------------------
// <copyright file="VJassScopeDeclarationSyntax.cs" company="Drake53">
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
    public class VJassScopeDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassScopeDeclarationSyntax(
            VJassScopeDeclaratorSyntax declarator,
            ImmutableArray<VJassScopedDeclarationSyntax> declarations,
            VJassSyntaxToken endScopeToken)
        {
            Declarator = declarator;
            Declarations = declarations;
            EndScopeToken = endScopeToken;
        }

        public VJassScopeDeclaratorSyntax Declarator { get; }

        public ImmutableArray<VJassScopedDeclarationSyntax> Declarations { get; }

        public VJassSyntaxToken EndScopeToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopeDeclarationSyntax scopeDeclaration
                && Declarator.IsEquivalentTo(scopeDeclaration.Declarator)
                && Declarations.IsEquivalentTo(scopeDeclaration.Declarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarator.WriteTo(writer);
            Declarations.WriteTo(writer);
            EndScopeToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Declarator.ProcessTo(writer, context);
            Declarations.ProcessTo(writer, context);
            EndScopeToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Declarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => Declarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndScopeToken;

        protected internal override VJassScopeDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopeDeclarationSyntax(
                Declarator.ReplaceFirstToken(newToken),
                Declarations,
                EndScopeToken);
        }

        protected internal override VJassScopeDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassScopeDeclarationSyntax(
                Declarator,
                Declarations,
                newToken);
        }
    }
}