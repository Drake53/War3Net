// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassFunctionDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassFunctionDeclaratorSyntax functionDeclarator,
            ImmutableArray<VJassStatementSyntax> statements,
            VJassSyntaxToken endFunctionToken)
        {
            Modifiers = modifiers;
            FunctionDeclarator = functionDeclarator;
            Statements = statements;
            EndFunctionToken = endFunctionToken;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassFunctionDeclaratorSyntax FunctionDeclarator { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public VJassSyntaxToken EndFunctionToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassFunctionDeclarationSyntax functionDeclaration
                && Modifiers.IsEquivalentTo(functionDeclaration.Modifiers)
                && FunctionDeclarator.IsEquivalentTo(functionDeclaration.FunctionDeclarator)
                && Statements.IsEquivalentTo(functionDeclaration.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            FunctionDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
            EndFunctionToken.WriteTo(writer);
        }

        public override string ToString() => $"{Modifiers.Join()}{FunctionDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)FunctionDeclarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndFunctionToken;

        protected internal override VJassFunctionDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassFunctionDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    FunctionDeclarator,
                    Statements,
                    EndFunctionToken);
            }

            return new VJassFunctionDeclarationSyntax(
                Modifiers,
                FunctionDeclarator.ReplaceFirstToken(newToken),
                Statements,
                EndFunctionToken);
        }

        protected internal override VJassFunctionDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionDeclarationSyntax(
                Modifiers,
                FunctionDeclarator,
                Statements,
                newToken);
        }
    }
}