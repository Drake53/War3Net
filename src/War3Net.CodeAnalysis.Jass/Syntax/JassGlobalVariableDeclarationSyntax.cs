// ------------------------------------------------------------------------------
// <copyright file="JassGlobalVariableDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalVariableDeclarationSyntax : JassGlobalDeclarationSyntax
    {
        internal JassGlobalVariableDeclarationSyntax(
            JassVariableOrArrayDeclaratorSyntax declarator)
        {
            Declarator = declarator;
        }

        public JassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalVariableDeclarationSyntax globalVariableDeclaration
                && Declarator.IsEquivalentTo(globalVariableDeclaration.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            Declarator.WriteTo(writer);
        }

        public override string ToString() => Declarator.ToString();

        public override JassSyntaxToken GetFirstToken() => Declarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override JassGlobalVariableDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalVariableDeclarationSyntax(Declarator.ReplaceFirstToken(newToken));
        }

        protected internal override JassGlobalVariableDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalVariableDeclarationSyntax(Declarator.ReplaceLastToken(newToken));
        }
    }
}