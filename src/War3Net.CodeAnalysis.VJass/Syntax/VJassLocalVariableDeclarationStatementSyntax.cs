// ------------------------------------------------------------------------------
// <copyright file="VJassLocalVariableDeclarationStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassLocalVariableDeclarationStatementSyntax : VJassStatementSyntax
    {
        internal VJassLocalVariableDeclarationStatementSyntax(
            VJassSyntaxToken localToken,
            VJassVariableOrArrayDeclaratorSyntax declarator)
        {
            LocalToken = localToken;
            Declarator = declarator;
        }

        public VJassSyntaxToken LocalToken { get; }

        public VJassVariableOrArrayDeclaratorSyntax Declarator { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement
                && Declarator.IsEquivalentTo(localVariableDeclarationStatement.Declarator);
        }

        public override void WriteTo(TextWriter writer)
        {
            LocalToken.WriteTo(writer);
            Declarator.WriteTo(writer);
        }

        public override string ToString() => $"{LocalToken} {Declarator}";

        public override VJassSyntaxToken GetFirstToken() => LocalToken;

        public override VJassSyntaxToken GetLastToken() => Declarator.GetLastToken();

        protected internal override VJassLocalVariableDeclarationStatementSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassLocalVariableDeclarationStatementSyntax(
                newToken,
                Declarator);
        }

        protected internal override VJassLocalVariableDeclarationStatementSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassLocalVariableDeclarationStatementSyntax(
                LocalToken,
                Declarator.ReplaceLastToken(newToken));
        }
    }
}