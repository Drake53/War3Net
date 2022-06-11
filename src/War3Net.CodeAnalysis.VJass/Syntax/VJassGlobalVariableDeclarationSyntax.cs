// ------------------------------------------------------------------------------
// <copyright file="VJassGlobalVariableDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassGlobalVariableDeclarationSyntax : VJassGlobalDeclarationSyntax
    {
        internal VJassGlobalVariableDeclarationSyntax(
            VJassVariableOrArrayDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public VJassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassGlobalVariableDeclarationSyntax globalVariableDeclaration
                && Declarator.IsEquivalentTo(globalVariableDeclaration.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarator.WriteTo(writer);
        }

        public override string ToString() => Declarator.ToString();

        public override VJassSyntaxToken GetFirstToken() => Declarator.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override VJassGlobalVariableDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalVariableDeclarationSyntax(Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override VJassGlobalVariableDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassGlobalVariableDeclarationSyntax(Declarator.ReplaceLastToken(newToken));
        }
    }
}