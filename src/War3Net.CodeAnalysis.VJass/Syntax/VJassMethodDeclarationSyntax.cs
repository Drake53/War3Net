// ------------------------------------------------------------------------------
// <copyright file="VJassMethodDeclarationSyntax.cs" company="Drake53">
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
    public class VJassMethodDeclarationSyntax : VJassMemberDeclarationSyntax
    {
        internal VJassMethodDeclarationSyntax(
            ImmutableArray<VJassModifierSyntax> modifiers,
            VJassMethodOrOperatorDeclaratorSyntax methodDeclarator,
            ImmutableArray<VJassStatementSyntax> statements,
            VJassSyntaxToken endMethodToken)
        {
            Modifiers = modifiers;
            MethodDeclarator = methodDeclarator;
            Statements = statements;
            EndMethodToken = endMethodToken;
        }

        public ImmutableArray<VJassModifierSyntax> Modifiers { get; }

        public VJassMethodOrOperatorDeclaratorSyntax MethodDeclarator { get; }

        public ImmutableArray<VJassStatementSyntax> Statements { get; }

        public VJassSyntaxToken EndMethodToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMethodDeclarationSyntax functionDeclaration
                && Modifiers.IsEquivalentTo(functionDeclaration.Modifiers)
                && MethodDeclarator.IsEquivalentTo(functionDeclaration.MethodDeclarator)
                && Statements.IsEquivalentTo(functionDeclaration.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            Modifiers.WriteTo(writer);
            MethodDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
            EndMethodToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Modifiers.ProcessTo(writer, context);
            MethodDeclarator.ProcessTo(writer, context);
            Statements.ProcessTo(writer, context);
            EndMethodToken.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Modifiers.Join()}{MethodDeclarator} [...]";

        public override VJassSyntaxToken GetFirstToken() => (Modifiers.IsEmpty ? (VJassSyntaxNode)MethodDeclarator : Modifiers[0]).GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => EndMethodToken;

        protected internal override VJassMethodDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (!Modifiers.IsEmpty)
            {
                return new VJassMethodDeclarationSyntax(
                    Modifiers.ReplaceFirstItem(Modifiers[0].ReplaceFirstToken(newToken)),
                    MethodDeclarator,
                    Statements,
                    EndMethodToken);
            }

            return new VJassMethodDeclarationSyntax(
                Modifiers,
                MethodDeclarator.ReplaceFirstToken(newToken),
                Statements,
                EndMethodToken);
        }

        protected internal override VJassMethodDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassMethodDeclarationSyntax(
                Modifiers,
                MethodDeclarator,
                Statements,
                newToken);
        }
    }
}